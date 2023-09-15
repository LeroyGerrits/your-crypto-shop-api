namespace DGBCommerce.Domain.Models
{
    public class ProductPhoto
    {
        public Guid? Id { get; set; }
        public required Guid ProductId { get; set; }
        public required string File { get; set; }
        public required string Extension { get; set; }
        public required int FileSize { get; set; }
        public required int Width { get; set; }
        public required int Height { get; set; }
        public string? Description { get; set; }
        public required int? SortOrder { get; set; }
        public required bool Main { get; set; }
        public required bool Visible { get; set; }
    }
}