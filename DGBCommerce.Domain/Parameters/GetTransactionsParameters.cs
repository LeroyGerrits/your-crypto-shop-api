namespace DGBCommerce.Domain.Parameters
{
    public class GetTransactionsParameters : GetParameters
    {
        public Guid? MerchantId { get; set; }
        public Guid? Id { get; set; }
        public Guid? ShopId { get; set; }
        public string? Recipient { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateUntil { get; set; }
        public string? Tx { get; set; }
        public bool? Unpaid { get; set; }
    }
}