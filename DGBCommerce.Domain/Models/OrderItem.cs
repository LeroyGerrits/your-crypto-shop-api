using DGBCommerce.Domain.Enums;

namespace DGBCommerce.Domain.Models
{
    public class OrderItem
    {
        public Guid? Id { get; set; }
        public required Guid OrderId { get; set; }
        public required OrderItemType Type { get; set; }
        public Guid? ProductId { get; set; }
        public required string Description { get; set; }
        public required uint Amount { get; set; }
        public required decimal Price { get; set; }

        public decimal Total
            => Amount * Price;
    }
}