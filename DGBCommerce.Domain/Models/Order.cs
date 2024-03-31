using DGBCommerce.Domain.Enums;

namespace DGBCommerce.Domain.Models
{
    public class Order
    {
        public Guid? Id { get; set; }
        public required Guid ShopId { get; set; }
        public required Customer Customer { get; set; }
        public DateTime? Date { get; set; }
        public required OrderStatus Status { get; set; }
        public required Address BillingAddress { get; set; }
        public required Address ShippingAddress { get; set; }
    }
}