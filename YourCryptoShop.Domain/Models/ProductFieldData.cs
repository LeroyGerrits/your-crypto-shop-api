namespace YourCryptoShop.Domain.Models
{
    public class ProductFieldData
    {
        public required Guid ProductId { get; set; }
        public required Guid FieldId { get; set; }
        public string Data { get; set; } = string.Empty;
    }
}