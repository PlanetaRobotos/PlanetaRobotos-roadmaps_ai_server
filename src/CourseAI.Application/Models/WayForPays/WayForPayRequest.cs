public class WayForPayRequest
{
    public string MerchantAccount { get; set; } = string.Empty;
    public string MerchantAuthType { get; set; } = string.Empty;
    public string MerchantDomainName { get; set; } = string.Empty;
    public string OrderReference { get; set; } = string.Empty;
    public string OrderDate { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string OrderTimeout { get; set; } = string.Empty;
    public string[] ProductName { get; set; } = Array.Empty<string>();
    public decimal[] ProductPrice { get; set; } = Array.Empty<decimal>();
    public int[] ProductCount { get; set; } = Array.Empty<int>();
    public string ServiceUrl { get; set; } = string.Empty;
}