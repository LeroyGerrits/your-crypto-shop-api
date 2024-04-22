namespace DGBCommerce.Domain.Models.ViewModels
{
    public class PublicDeliveryMethod
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public decimal? Costs { get; set; }
        public Dictionary<Guid, decimal> CostsPerCountry { get; set; } = [];
    }
}