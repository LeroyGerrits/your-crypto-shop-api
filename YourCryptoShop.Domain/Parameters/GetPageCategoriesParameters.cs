namespace YourCryptoShop.Domain.Parameters
{
    public class GetPageCategoriesParameters : GetParameters
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
    }
}