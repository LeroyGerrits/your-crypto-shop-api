namespace DGBCommerce.Domain.Models
{
    public class FaqCategory
    {
        public Guid? Id { get; set; }
        public FaqCategory? Parent { get; set; }
        public required string Name { get; set; }
        public List<FaqCategory>? Children { get; }
        public int? SortOrder { get; set; }
    }
}