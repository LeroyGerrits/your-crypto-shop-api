using YourCryptoShop.Domain.Enums;

namespace YourCryptoShop.Domain.Models
{
    public class Currency
    {
        public Guid? Id { get; set; }
        public string? Symbol { get; set; }
        public required string Code { get; set; }
        public required string Name { get; set; }
        public required CurrencyType Type { get; set; }
    }
}