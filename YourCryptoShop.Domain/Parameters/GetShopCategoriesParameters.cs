namespace YourCryptoShop.Domain.Parameters
{
    public class GetShopCategoriesParameters : GetParameters
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
    }
}