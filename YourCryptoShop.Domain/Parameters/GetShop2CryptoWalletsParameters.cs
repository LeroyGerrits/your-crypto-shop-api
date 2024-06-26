namespace YourCryptoShop.Domain.Parameters
{
    public class GetShop2CryptoWalletsParameters : GetParameters
    {
        public Guid? MerchantId { get; set; }
        public Guid? ShopId { get; set; }
        public Guid? CryptoWalletId { get; set; }
    }
}