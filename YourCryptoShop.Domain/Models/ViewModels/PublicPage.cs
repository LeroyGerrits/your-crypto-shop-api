namespace YourCryptoShop.Domain.Models.ViewModels
{
    public class PublicPage
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Content { get; set; }
        public List<Guid>? CategoryIds { get; set; }
        public required bool Index { get; set; }
    }
}