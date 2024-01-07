namespace DGBCommerce.Domain.Models
{
    public class Page
    {
        public Guid? Id { get; set; }
        public required Shop Shop { get; set; }
        public required string Title { get; set; }        
        public string? Content { get; set; }
        public required bool Visible { get; set; }
    }
}