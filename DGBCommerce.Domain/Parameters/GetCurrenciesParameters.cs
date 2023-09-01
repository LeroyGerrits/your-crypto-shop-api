namespace DGBCommerce.Domain.Parameters
{
    public class GetCurrenciesParameters : GetParameters
    {
        public Guid? Id { get; set; }
        public string? Symbol { get; set; }
        public string? Name { get; set; }
    }
}