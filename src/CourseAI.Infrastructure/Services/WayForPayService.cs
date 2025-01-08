using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using CourseAI.Application.Models.WayForPays;
using Microsoft.Extensions.Configuration;
using NeerCore.DependencyInjection;

namespace CourseAI.Infrastructure.Services;

[Service]
public class WayForPayService : IWayForPayService
{
    private readonly string _merchantKey;
    private readonly string _merchantAccount;
    
    public WayForPayService(IConfiguration configuration)
    {
        _merchantKey = configuration["WayForPay:MerchantKey"];
        _merchantAccount = configuration["WayForPay:MerchantAccount"];
    }

    public string GenerateSignature(WayForPayRequest request)
    {
        var stringToHash = string.Join(";", new[]
        {
            _merchantAccount,         // test_merchant
            request.MerchantDomainName,      // www.market.ua
            request.OrderReference,          // DH783023
            request.OrderDate,               // 1415379863
            request.Amount.ToString(CultureInfo.InvariantCulture), // 1547.36
            request.Currency,                // UAH
            request.ProductName[0],          // Процесор Intel Core i5-4670 3.4GHz
            request.ProductCount[0].ToString(), // 1
            request.ProductPrice[0].ToString(CultureInfo.InvariantCulture), // 1000.00
        });

        // Use HMAC-MD5 instead of plain MD5
        using var hmac = new HMACMD5(Encoding.UTF8.GetBytes(_merchantKey));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
    
        // Convert to hex string (not base64)
        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }
    
    public bool VerifyCallback(WayForPayResponse response)
    {
        // Implement signature verification logic
        return true; // Replace with actual verification
    }
}