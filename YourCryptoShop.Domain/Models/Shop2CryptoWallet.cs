namespace YourCryptoShop.Domain.Models
{
    public class Shop2CryptoWallet
    {
        public required Guid ShopId { get; set; }
        public required Guid CryptoWalletId { get; set; }
    }
}