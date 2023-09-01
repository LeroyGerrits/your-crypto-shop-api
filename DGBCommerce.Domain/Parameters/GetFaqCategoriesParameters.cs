namespace DGBCommerce.Domain.Parameters
{
    public class GetFaqCategoriesParameters : GetParameters
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
    }
}