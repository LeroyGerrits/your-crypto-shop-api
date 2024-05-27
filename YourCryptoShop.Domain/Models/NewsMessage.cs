namespace YourCryptoShop.Domain.Models
{
    public class NewsMessage
    {
        public Guid? Id { get; set; }
        public required DateTime Date { get; set; }
        public string? ThumbnailUrl { get; set; }
        public required string Title { get; set; }
        public required string Intro { get; set; }
        public string? Content { get; set; }
    }
}