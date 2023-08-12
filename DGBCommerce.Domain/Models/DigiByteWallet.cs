namespace DGBCommerce.Domain.Models
{
    public class DigiByteWallet
    {
        public Guid? Id { get; set; }
        public required Merchant Merchant { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
    }
}