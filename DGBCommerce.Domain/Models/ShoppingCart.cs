namespace DGBCommerce.Domain.Models
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
        public uint CumulativeItems { get; set; }
    }
}