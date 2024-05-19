namespace DGBCommerce.Domain.Models
{
    public class ShoppingCartItemFieldData
    {
        public required Guid ShoppingCartItemId { get; set; }
        public required Guid FieldId { get; set; }
        public string Data { get; set; } = string.Empty;
    }
}