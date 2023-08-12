namespace DGBCommerce.Domain.Parameters
{
    public class GetDigiByteWalletsParameters
    {
        public Guid? Id { get; set; }
        public Guid? MerchantId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
    }
}