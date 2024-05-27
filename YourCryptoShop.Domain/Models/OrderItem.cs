using YourCryptoShop.Domain.Enums;

namespace YourCryptoShop.Domain.Models
{
    public class OrderItem
    {
        public Guid? Id { get; set; }
        public required Guid OrderId { get; set; }
        public required OrderItemType Type { get; set; }
        public Guid? ProductId { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public decimal? ProductPrice { get; set; }
        public required string Description { get; set; }
        public required uint Amount { get; set; }
        public required decimal Price { get; set; }

        public decimal Total
            => Amount * Price;
    }
}