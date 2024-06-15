namespace YourCryptoShop.Domain.Models
{
    public class CurrencyRate
    {
        public required Guid Id { get; set; }
        public required Guid CurrencyFromId { get; set; }
        public required Guid CurrencyToId { get; set; }
        public required decimal Rate { get; set; }
        public required DateTime Date { get; set; }
    }
}