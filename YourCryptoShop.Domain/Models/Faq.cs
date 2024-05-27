namespace YourCryptoShop.Domain.Models
{
    public class Faq
    {
        public Guid? Id { get; set; }
        public required FaqCategory Category { get; set; }
        public required string Title { get; set; }
        public string[]? Keywords { get; set; }
        public string? Content { get; set; }
        public int? SortOrder { get; set; }
    }
}