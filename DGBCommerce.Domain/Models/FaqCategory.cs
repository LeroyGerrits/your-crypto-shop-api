namespace DGBCommerce.Domain.Models
{
    public class FaqCategory
    {
        public Guid? Id { get; set; }
        public required string Name { get; set; }
        public int? SortOrder { get; set; }
    }
}