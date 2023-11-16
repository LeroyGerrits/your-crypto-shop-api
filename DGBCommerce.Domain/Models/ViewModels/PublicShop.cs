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
        public Guid? CountryId { get; set; }
        public string? CountryCode { get; set; }
        public string? CountryName { get; set; }
        public Guid? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public bool Featured { get; set; }
    }
}