using YourCryptoShop.Domain.Models;

namespace YourCryptoShop.API.Controllers.Requests
{
    public class MutateShoppingCartItemRequest
    {
        public required Guid ShopId { get; set; }
        public required ShoppingCartItem ShoppingCartItem { get; set; }
        public Dictionary<Guid, string?>? FieldData { get; set; }
    }
}