namespace DGBCommerce.Domain.Parameters
{
    public class GetShoppingCartItemFieldDataParameters : GetParameters
    {
        public Guid? ShoppingCartId { get; set; }
        public Guid? ShoppingCartItemId { get; set; }
        public Guid? FieldId { get; set; }
    }
}