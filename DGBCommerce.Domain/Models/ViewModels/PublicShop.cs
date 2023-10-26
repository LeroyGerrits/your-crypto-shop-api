namespace DGBCommerce.Domain.Models.ViewModels
{
    public class PublicShop
    {
        public Guid? Id { get; set; }
        public required Guid MerchantId { get; set; }
        public required string MerchantUsername { get; set; }
        public decimal? MerchantScore { get; set; }
        public required string Name { get; set; }
        public string? SubDomain { get; set; }
        public bool Featured { get; set; }
    }
}