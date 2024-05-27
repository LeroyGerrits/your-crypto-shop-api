using YourCryptoShop.Domain.Enums;

namespace YourCryptoShop.Domain.Parameters
{
    public class GetCurrenciesParameters : GetParameters
    {
        public Guid? Id { get; set; }
        public CurrencyType? Type { get; set; }
        public string? Symbol { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
    }
}