namespace DGBCommerce.Domain.Models
{
    public class Stats
    {
        public required int Merchants { get; set; }
        public required int Shops { get; set; }
        public required int Orders { get; set; }
        public required int Transactions { get; set; }
    }
}