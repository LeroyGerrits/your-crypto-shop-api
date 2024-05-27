namespace YourCryptoShop.Domain.Models
{
    public class Category
    {
        public Guid? Id { get; set; }
        public required Guid ShopId { get; set; }
        public Guid? ParentId { get; set; }
        public required string Name { get; set; }
        public required bool Visible { get; set; }
        public int? SortOrder { get; set; }
        public List<Category>? Children { get; set; }
    }
}