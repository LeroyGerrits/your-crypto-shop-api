namespace YourCryptoShop.Domain.Models
{
    public class ShoppingCart
    {
        public Guid? Id { get; set; }
        public required Guid Session { get; set; }
        public Guid? CustomerId { get; set; }
        public string? LastIpAddress { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime Edited { get; set; } = DateTime.UtcNow;
        public List<ShoppingCartItem>? Items { get; set; }

        public uint CumulativeAmount
            => this.Items != null ? (uint)this.Items.Sum(item => item.Amount) : 0;

        public decimal CumulativeTotal
            => this.Items != null ? this.Items.Sum(item => item.Total) : 0;
    }
}