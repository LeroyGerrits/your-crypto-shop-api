namespace DGBCommerce.Domain.Models
{
    public class Transaction
    {
        public Guid? Id { get; set; }
        public required Guid ShopId { get; set; }
        public DateTime Date { get; set; }
        public required string Recipient { get; set; }
        public required decimal AmountDue { get; set; }        
        public required decimal AmountPaid { get; set; }
        public DateTime? PaidInFull { get; set; }
    }
}