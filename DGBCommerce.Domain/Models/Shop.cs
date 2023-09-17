namespace DGBCommerce.Domain.Models
{
    public class Shop
    {
        public Guid? Id { get; set; }
        public required Guid MerchantId { get; set; }
        public required string Name { get; set; }
        public string? SubDomain { get; set; }
        public bool Featured { get; set; }
    }
}