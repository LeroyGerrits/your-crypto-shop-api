namespace DGBCommerce.Domain.Models
{
    public class ShoppingCart
    {
        public required Guid Id { get; set; }
        public required Guid Session { get; set; }
        public Guid? CustomerId { get; set; }
        public string? LastIpAddress { get; set; }
        public DateTime Created { get; set; }
        public DateTime Edited { get; set; }
    }
}