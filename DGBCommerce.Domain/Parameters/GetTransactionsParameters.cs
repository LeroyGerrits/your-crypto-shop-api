namespace DGBCommerce.Domain.Parameters
{
    public class GetTransactionsParameters : GetParameters
    {
        public Guid? Id { get; set; }
        public Guid? ShopId { get; set; }
        public string? Recipient { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateUntil { get; set; }
    }
}