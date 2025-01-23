using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using CourseAI.Api.Core;
using CourseAI.Api.Extensions;
using CourseAI.Application.Extensions;
using CourseAI.Application.Features.Purchases;
using CourseAI.Application.Models;
using CourseAI.Application.Models.WayForPays;
using CourseAI.Application.Services;
using CourseAI.Core.Security;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities;
using CourseAI.Domain.Entities.Identity;
using CourseAI.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CourseAI.Api.Controllers;

public class PurchaseController(
    IWayForPayService wayForPayService,
    IConfiguration configuration,
    IUserService userService,
    IRoleService roleService,
    AppDbContext dbContext,
    UserManager<User> userManager,
    IJwtProvider jwtProvider
) : V1Controller
{
    [HttpPost("create")]
    public IActionResult CreatePayment([FromBody] CreatePaymentRequest request)
    {
        var utcNow = DateTime.UtcNow;
        var orderReference = $"ORDER_{DateTime.Now:yyyyMMddHHmmss}";

        if (!Enum.IsDefined(typeof(Roles), request.PlanType))
            throw new Exception($"Invalid plan selected. PlanType: {request.PlanType}");

        var userPurchase = new UserPurchase
        {
            Id = Guid.NewGuid(),
            CreatedOnUtc = utcNow,
            OrderReference = orderReference,
            Role = Enum.Parse<Roles>(request.PlanType, true),
            ActiveEmail = request.Email
        };

        dbContext.UserPurchases.Add(userPurchase);
        dbContext.SaveChanges();

        var merchantAccount = configuration["WayForPay:MerchantAccount"];
        var clientPath = configuration["Client:Url"];
        var price = request.PlanPrice;

        var wayforpayRequest = new WayForPayRequest
        {
            MerchantAccount = merchantAccount,
            MerchantAuthType = "SimpleSignature",
            MerchantDomainName = "levenue.tech",
            OrderReference = orderReference,
            OrderDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
            Amount = price,
            Currency = "USD",
            OrderTimeout = "49000",
            ProductName =
            [
                $"Access forever to Levenue MiniCourses. {request.PlanType.FirstLetterToUpper()} Plan: {request.PlanDescription}"
            ],
            ProductPrice = [price],
            ProductCount = [1],
            ClientEmail = request.Email,
            ServiceUrl = "https://app-241207145936.azurewebsites.net/v1/purchase/callback",
            ReturnUrl = $"{clientPath}/callback"
        };

        var signature = wayForPayService.GenerateSignature(wayforpayRequest);

        return Ok(new
        {
            merchantAccount = wayforpayRequest.MerchantAccount,
            merchantAuthType = wayforpayRequest.MerchantAuthType,
            merchantDomainName = wayforpayRequest.MerchantDomainName,
            merchantSignature = signature,
            orderReference = wayforpayRequest.OrderReference,
            orderDate = wayforpayRequest.OrderDate,
            amount = wayforpayRequest.Amount,
            currency = wayforpayRequest.Currency,
            orderTimeout = wayforpayRequest.OrderTimeout,
            productName = wayforpayRequest.ProductName,
            productPrice = wayforpayRequest.ProductPrice,
            productCount = wayforpayRequest.ProductCount,
            clientEmail = wayforpayRequest.ClientEmail,
            serviceUrl = wayforpayRequest.ServiceUrl,
            returnUrl = wayforpayRequest.ReturnUrl
        });
    }

    [HttpPost("login-by-payment-details")]
    public async Task<IActionResult> LoginByPaymentDetails([FromBody] PaymentLoginRequest request)
    {
        var userPurchase = dbContext.UserPurchases.FirstOrDefault(x => x.OrderReference == request.OrderReference);
        if (userPurchase == null)
            return NotFound("User purchase not found.");

        // user return from payment page without payment
        if (!userPurchase.IsActivated)
        {
            dbContext.UserPurchases.Remove(userPurchase);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        if (userPurchase.ActiveEmail == null)
            return NotFound("User email not found.");

        var user = await userManager.FindByEmailAsync(userPurchase.ActiveEmail);
        if (user == null)
            return NotFound("User not found.");

        var jwtToken = jwtProvider.Create(user);
        dbContext.UserPurchases.Remove(userPurchase);
        await dbContext.SaveChangesAsync();

        return Ok(new { token = jwtToken });
    }

    [HttpPost("callback")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<IActionResult> HandleCallback()
    {
        var merchantSecretKey = configuration["WayForPay:MerchantKey"];
        WayForPayResponse? response = null;
        var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var status = "accept";

        try
        {
            Logger.LogInformation("Headers: {@Headers}", Request.Headers);

            var formDict = Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
            Logger.LogInformation("Form Data: {@FormData}", formDict);

            var jsonString = formDict.Keys.FirstOrDefault();
            if (string.IsNullOrEmpty(jsonString))
            {
                Logger.LogWarning("No JSON data received");
                throw new Exception("Invalid payload");
            }

            response = JsonSerializer.Deserialize<WayForPayResponse>(jsonString);

            if (response == null)
            {
                Logger.LogWarning("Failed to deserialize response");
                throw new Exception("Invalid payload format");
            }

            Logger.LogInformation("Deserialized response: {@Response}", response);

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                Logger.LogError("Model binding failed: {Errors}", errors);
                throw new Exception("Invalid callback payload.");
            }

            if (string.IsNullOrEmpty(response.TransactionStatus))
            {
                Logger.LogError("Transaction status is missing");
                throw new Exception("Transaction status is missing");
            }

            var userPurchase = dbContext.UserPurchases.FirstOrDefault(x => x.OrderReference == response.OrderReference);
            if (userPurchase == null)
            {
                Logger.LogError("User purchase not found for order reference {OrderReference}",
                    response.OrderReference);
                throw new Exception("User purchase not found.");
            }

            var planType = userPurchase.Role.ToString();

            switch (response.TransactionStatus.ToLower())
            {
                case "approved":
                    Logger.LogInformation(
                        $"Payment for order {response.OrderReference} was successful. PlanType: {planType}");

                    if (!Enum.IsDefined(typeof(Roles), planType))
                        throw new Exception($"Invalid plan selected. PlanType: {planType}");

                    User? user;
                    // user had no email before
                    if (userPurchase.ActiveEmail == null)
                    {
                        if (response.Email == null)
                            throw new Exception("response email is missing");

                        user = await userService.CreateUser(response.Email, false, [Roles.user.ToString(), planType]);
                        if (user == null)
                        {
                            throw new Exception("Failed to create user.");
                        }

                        Logger.LogInformation($"User {user.Email} created with role {planType}");
                        userPurchase.ActiveEmail = response.Email;
                    }
                    else
                    {
                        user = await userManager.FindByEmailAsync(userPurchase.ActiveEmail);
                        if (user == null)
                            throw new Exception($"User not found, email: {userPurchase.ActiveEmail}");

                        var assignResult = await roleService.AssignRoleAsync(user.Id, planType);
                        if (!assignResult)
                            throw new Exception($"Failed to assign role.");

                        Logger.LogInformation($"Role {planType} assigned to user {user.Email}");
                    }

                    userPurchase.IsActivated = true;
                    await dbContext.SaveChangesAsync();
                    break;
                case "declined":
                    Logger.LogInformation($"Payment for order {response.OrderReference} was declined");
                    break;
                case "refunded":
                    Logger.LogInformation($"Payment for order {response.OrderReference} was refunded");
                    break;
            }

            var signatureString = $"{response.OrderReference};{status};{time}";

            Logger.LogInformation($"Signature string: {signatureString}: {merchantSecretKey}");

            var signature = CalculateHmac(signatureString, merchantSecretKey);

            return Ok(new
            {
                orderReference = response.OrderReference,
                status = status,
                time = time,
                signature = signature
            });
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error processing WayForPay callback");

            var signatureString = $"{response?.OrderReference ?? ""};{status};{time}";
            var signature = CalculateHmac(signatureString, merchantSecretKey);

            Logger.LogInformation(
                $"Signature string: {signatureString}, signature: {signature}, secret: {merchantSecretKey}");

            return Ok(new
            {
                orderReference = response?.OrderReference ?? "",
                status = status,
                time = time,
                signature = signature
            });
        }
    }

    private static string CalculateHmac(string input, string key)
    {
        using var hmac = new HMACMD5(Encoding.UTF8.GetBytes(key));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }

    [HttpPost("buy-plan")]
    [Authorize]
    public async Task<IActionResult> BuyPlan([FromBody] BuyPlanRequest request)
    {
        var response = await Sender.Send(request);
        return response.MatchEmptyResponse();
    }

    [HttpDelete("delete-plan")]
    [Authorize]
    public async Task<IActionResult> DeletePlan([FromBody] DeletePlanRequest request)
    {
        var response = await Sender.Send(request);
        return response.MatchEmptyResponse();
    }

    [HttpPost("update-default-users")]
    public async Task<IActionResult> UpdateDefaultUsers([FromBody] UpdateDefaultUsersRequest request)
    {
        var response = await Sender.Send(request);
        return response.MatchEmptyResponse();
    }

    [HttpPost("update-users-tokens")]
    public async Task<IActionResult> UpdateUsersTokens([FromBody] UpdateUsersTokensRequest request)
    {
        var response = await Sender.Send(request);
        return response.MatchEmptyResponse();
    }
}