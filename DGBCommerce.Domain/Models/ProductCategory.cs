namespace DGBCommerce.Domain.Models
{
    public class ProductCategory
    {
        public required Guid ProductId { get; set; }
        public required Guid CategoryId { get; set; }
    }
}