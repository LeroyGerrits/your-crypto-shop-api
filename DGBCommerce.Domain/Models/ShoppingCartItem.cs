namespace DGBCommerce.Domain.Models
{
    public class ShoppingCartItem
    {
        public Guid? Id { get; set; }
        public required Guid ShoppingCartId { get; set; }
        public required Guid ProductId { get; set; }
        public required string ProductName { get; set; }
        public required decimal ProductPrice { get; set; }
        public int? ProductStock { get; set; }
        public required uint Amount { get; set; }

        public decimal Total
            => Amount * ProductPrice;
    }
}