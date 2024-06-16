namespace YourCryptoShop.Domain.Models
{
    public class CurrencyRate
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required Guid CurrencyFromId { get; set; }
        public required Guid CurrencyToId { get; set; }
        public required decimal Rate { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public decimal InvertedRate
            => 1 / this.Rate;
    }
}