namespace YourCryptoShop.Domain.Parameters
{
    public class GetDigiByteWalletsParameters : GetParameters
    {
        public Guid MerchantId { get; set; }
        public Guid? Id { get; set; }        
        public string? Name { get; set; }
        public string? Address { get; set; }
    }
}