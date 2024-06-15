namespace YourCryptoShop.Domain.Models.Response.CryptoCompare
{
    public class GetRatesResponse
    {
        public required Dictionary<string, decimal> BCH { get; set; }
        public required Dictionary<string, decimal> DCR { get; set; }
        public required Dictionary<string, decimal> DOGE { get; set; }
        public required Dictionary<string, decimal> LTC { get; set; }
        public required Dictionary<string, decimal> XMR { get; set; }
    }
}