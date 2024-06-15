namespace YourCryptoShop.Domain.Parameters
{
    public class GetCurrencyRatesParameters : GetParameters
    {
        public Guid? Id { get; set; }
        public Guid? CurrencyFromId { get; set; }
        public Guid? CurrencyToId { get; set; }
    }
}