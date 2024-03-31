namespace DGBCommerce.Domain.Models
{
    public class Shop
    {
        public Guid? Id { get; set; }
        public required Guid MerchantId { get; set; }
        public required string Name { get; set; }
        public string? SubDomain { get; set; }
        public Country? Country { get; set; }
        public ShopCategory? Category { get; set; }
        public bool Featured { get; set; }
        public DigiByteWallet? Wallet { get; set; }
    }
}