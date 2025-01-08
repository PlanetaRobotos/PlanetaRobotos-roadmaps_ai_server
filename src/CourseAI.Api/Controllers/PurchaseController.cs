using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using CourseAI.Api.Core;
using CourseAI.Api.Extensions;
using CourseAI.Application.Features.Purchases;
using CourseAI.Application.Models.WayForPays;
using CourseAI.Application.Services;
using CourseAI.Core.Security;
using CourseAI.Domain.Context;
using CourseAI.Domain.Entities;
using CourseAI.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseAI.Api.Controllers;

public class PurchaseController(
    IWayForPayService wayForPayService,
    IConfiguration configuration,
    IUserService userService,
    IRoleService roleService,
    AppDbContext dbContext
) : V1Controller
{
    [HttpPost("create")]
    public IActionResult CreatePayment([FromBody] CreatePaymentRequest request)
    {
        var utcNow = DateTime.UtcNow;
        var orderReference = $"ORDER_{DateTime.Now:yyyyMMddHHmmss}";

        if (!Enum.IsDefined(typeof(Roles), request.PlanType))
            return BadRequest($"Invalid plan selected. PlanType: {request.PlanType}");

        var userPurchase = new UserPurchase
        {
            Id = Guid.NewGuid(),
            CreatedOnUtc = utcNow,
            OrderReference = orderReference,
            Role = Enum.Parse<Roles>(request.PlanType)
        };

        dbContext.UserPurchases.Add(userPurchase);
        dbContext.SaveChanges();

        var merchantAccount = configuration["WayForPay:MerchantAccount"];
        var wayforpayRequest = new WayForPayRequest
        {
            MerchantAccount = merchantAccount,
            MerchantAuthType = "SimpleSignature",
            MerchantDomainName = "levenue.tech",
            OrderReference = orderReference,
            OrderDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
            Amount = 1,
            Currency = "USD",
            OrderTimeout = "49000",
            ProductName =
            [
                "Standard Plan"
            ],
            ProductPrice =
            [
                45
            ],
            ProductCount =
            [
                1
            ],
            ServiceUrl = "https://app-241207145936.azurewebsites.net/v1/purchase/callback",
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
            serviceUrl = wayforpayRequest.ServiceUrl,
        });
    }

    [HttpPost("callback")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<IActionResult> HandleCallback()
    {
        var merchantSecretKey = configuration["WayForPay:MerchantKey"];
        WayForPayResponse? response = null;

        try
        {
            Logger.LogInformation("Headers: {@Headers}", Request.Headers);

            var formDict = Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
            Logger.LogInformation("Form Data: {@FormData}", formDict);

            var jsonString = formDict.Keys.FirstOrDefault();
            if (string.IsNullOrEmpty(jsonString))
            {
                Logger.LogWarning("No JSON data received");
                return BadRequest("Invalid payload");
            }

            response = JsonSerializer.Deserialize<WayForPayResponse>(jsonString);

            if (response == null)
            {
                Logger.LogWarning("Failed to deserialize response");
                return BadRequest("Invalid payload format");
            }

            Logger.LogInformation("Deserialized response: {@Response}", response);

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                Logger.LogError("Model binding failed: {Errors}", errors);
                return BadRequest("Invalid callback payload.");
            }

            var userPurchase = dbContext.UserPurchases.FirstOrDefault(x => x.OrderReference == response.OrderReference);
            if (userPurchase == null)
            {
                Logger.LogError("User purchase not found for order reference {OrderReference}",
                    response.OrderReference);
                return BadRequest("User purchase not found.");
            }

            var planType = userPurchase.Role.ToString();

            if (!string.IsNullOrEmpty(response.TransactionStatus))
            {
                switch (response.TransactionStatus?.ToLower())
                {
                    case "approved":
                        Logger.LogInformation(
                            $"Payment for order {response.OrderReference} was successful. PlanType: {planType}");

                        if (!Enum.IsDefined(typeof(Roles), planType))
                            return BadRequest($"Invalid plan selected. PlanType: {planType}");

                        var userResult = await userService.GetUser();
                        var user = userResult.Match(
                            user => user,
                            error => throw new Exception(error.Message)
                        );

                        // Assigning the selected role
                        var convertedUserId = Convert.ToInt64(user.Id);
                        var assignResult = await roleService.AssignRoleAsync(convertedUserId, planType);
                        if (!assignResult)
                            return BadRequest($"Failed to assign role.");

                        Logger.LogInformation($"Role {planType} assigned to user {user.Email}");
                        break;
                    case "declined":
                        Logger.LogInformation($"Payment for order {response.OrderReference} was declined");
                        break;
                    case "refunded":
                        Logger.LogInformation($"Payment for order {response.OrderReference} was refunded");
                        break;
                }
            }

            var time = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            var status = "accept";

            var signatureString = $"{response.OrderReference};{status};{time}";

            Logger.LogInformation($"Signature string: {signatureString}, Secret Key: {merchantSecretKey}");

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

            var time = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            const string status = "failure";

            // Even for failures, we need to sign the response
            var signatureString = $"{response?.OrderReference ?? ""};{status};{time}";
            var signature = CalculateHmac(signatureString, merchantSecretKey);

            return StatusCode(500, new
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