using System.Text.Json;
using System.Text.Json.Serialization;

namespace CourseAI.Application.Models.WayForPays;

public class WayForPayResponse
{
    [JsonPropertyName("merchantAccount")]
    public string? MerchantAccount { get; set; }

    [JsonPropertyName("orderReference")]
    public string? OrderReference { get; set; }

    [JsonPropertyName("merchantSignature")]
    public string? MerchantSignature { get; set; }

    [JsonPropertyName("amount")]
    public decimal? Amount { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("authCode")]
    public string? AuthCode { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("cardPan")]
    public string? CardPan { get; set; }

    [JsonPropertyName("cardType")]
    public string? CardType { get; set; }

    [JsonPropertyName("transactionStatus")]
    public string? TransactionStatus { get; set; }

    [JsonPropertyName("reason")]
    public string? Reason { get; set; }

    [JsonPropertyName("reasonCode")]
    public int? ReasonCode { get; set; }

    [JsonPropertyName("fee")]
    public decimal? Fee { get; set; }

    [JsonPropertyName("paymentSystem")]
    public string? PaymentSystem { get; set; }

    [JsonPropertyName("products")]
    public List<Product>? Products { get; set; }
}

public class Product
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("price")]
    public decimal? Price { get; set; }

    [JsonPropertyName("count")]
    public int? Count { get; set; }
}