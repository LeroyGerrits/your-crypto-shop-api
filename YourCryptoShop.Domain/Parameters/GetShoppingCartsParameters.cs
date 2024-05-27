namespace YourCryptoShop.Domain.Parameters
{
    public class GetShoppingCartsParameters : GetParameters
    {
        public Guid? Id { get; set; }
        public Guid? Session { get; set; }
        public Guid? CustomerId { get; set; }
    }
}