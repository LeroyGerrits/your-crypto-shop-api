namespace DGBCommerce.Domain.Parameters
{
    public class GetCustomersParameters : GetParameters
    {
        public required Guid MerchantId { get; set; }
        public Guid? Id { get; set; }        
        public Guid? ShopId { get; set; }
        public string? EmailAddress { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}