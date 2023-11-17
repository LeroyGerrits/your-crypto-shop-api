namespace DGBCommerce.Domain.Models
{
    public class Product2Category
    {
        public required Guid ProductId { get; set; }
        public required Guid CategoryId { get; set; }
    }
}