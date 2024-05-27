namespace YourCryptoShop.Domain.Models
{
    public class DeliveryMethod
    {
        public Guid? Id { get; set; }
        public required Shop Shop { get; set; }
        public required string Name { get; set; }
        public decimal? Costs { get; set; }
    }
}