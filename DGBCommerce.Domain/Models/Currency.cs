namespace DGBCommerce.Domain.Models
{
    public class Currency
    {
        public Guid? Id { get; set; }
        public required string Symbol { get; set; }
        public required string Name { get; set; }
    }
}