using YourCryptoShop.Domain.Enums;

namespace YourCryptoShop.Domain.Models
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
        public required Guid CurrencyId { get; set; }
        public string? Comments { get; set; }
        public string? SenderWalletAddress { get; set; }
        public Transaction? Transaction { get; set; }
        public List<OrderItem>? Items { get; set; }

        public uint CumulativeAmount
            => this.Items != null ? (uint)this.Items.Sum(item => item.Amount) : 0;

        public decimal CumulativeTotal
            => this.Items != null ? this.Items.Sum(item => item.Total) : 0;
    }
}