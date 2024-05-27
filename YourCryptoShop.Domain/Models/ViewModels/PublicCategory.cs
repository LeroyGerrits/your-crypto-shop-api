namespace YourCryptoShop.Domain.Models.ViewModels
{
    public class PublicCategory
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public required string Name { get; set; }
        public List<PublicCategory>? Children { get; set; }
    }
}