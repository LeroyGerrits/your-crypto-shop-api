namespace DGBCommerce.Domain.Models
{
    public class Transaction
    {
        public Guid? Id { get; set; }
        public required Guid ShopId { get; set; }
        public DateTime Date { get; set; }
        public required decimal Amount { get; set; }
        public required string Recipient { get; set; }
        public decimal? Paid { get; set; }
        public DateTime? PayDate { get; set; }
    }
}