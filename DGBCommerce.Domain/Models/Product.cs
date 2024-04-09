namespace DGBCommerce.Domain.Models
{
    public class Product
    {
        public Guid? Id { get; set; }
        public required Guid ShopId { get; set; }
        public string? Code { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int? Stock { get; set; }
        public required decimal Price { get; set; }
        public required bool Visible { get; set; }
        public required bool ShowOnHome { get; set; }
        public Guid? MainPhotoId { get; set; }
        public string? MainPhotoExtension { get; set; }
    }
}