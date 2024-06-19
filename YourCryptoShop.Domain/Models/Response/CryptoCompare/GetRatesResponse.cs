namespace YourCryptoShop.Domain.Models.Response.CryptoCompare
{
    public class GetRatesResponse
    {
        public required Dictionary<string, decimal> AUD { get; set; }
        public required Dictionary<string, decimal> CAD { get; set; }
        public required Dictionary<string, decimal> EUR { get; set; }
        public required Dictionary<string, decimal> GBP { get; set; }
        public required Dictionary<string, decimal> USD { get; set; }
    }
}