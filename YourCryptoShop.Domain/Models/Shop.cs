using YourCryptoShop.Domain.Enums;

namespace YourCryptoShop.Domain.Models
{
    public class Shop
    {
        public Guid? Id { get; set; }
        public required Guid MerchantId { get; set; }
        public required string Name { get; set; }
        public string? SubDomain { get; set; }
        public Country? Country { get; set; }
        public ShopCategory? Category { get; set; }        
        public DigiByteWallet? Wallet { get; set; }
        public ShopOrderMethod OrderMethod { get; set; }
        public bool RequireAddresses { get; set; }
        public bool Featured { get; set; }
    }
}