using DGBCommerce.Domain.Enums;

namespace DGBCommerce.Domain.Models
{
    public class Order
    {
        public Guid? Id { get; set; }
        public required Shop Shop { get; set; }
        public required Customer Customer { get; set; }
        public DateTime Date { get; set; }
        public required OrderStatus Status { get; set; }
        public required Address BillingAddress { get; set; }
        public required Address ShippingAddress { get; set; }
        public required Guid DeliveryMethodId { get; set; }
        public string? Comments { get; set; }
        public string? SenderWalletAddress { get; set; }
        public Transaction? Transaction { get; set; }
    }
}