namespace DGBCommerce.Domain.Models.ViewModels
{
    public class PublicProductPhoto
    {        
        public Guid Id { get; set; }
        public required string File { get; set; }
        public required string Extension { get; set; }
        public required int FileSize { get; set; }
        public required int Width { get; set; }
        public required int Height { get; set; }
        public string? Description { get; set; }
        public int? SortOrder { get; set; }
        public bool? Main { get; set; }
    }
}