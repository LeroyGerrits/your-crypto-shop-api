namespace DGBCommerce.Domain.Parameters
{
    public class GetShoppingCartItemsParameters : GetParameters
    {
        public Guid? Id { get; set; }
        public Guid? ShoppingCartId { get; set; }
        public Guid? ProductId { get; set; }
    }
}