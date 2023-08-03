namespace DGBCommerce.Domain.Parameters
{
    public class GetMerchantsParameters
    {
        public Guid? Id { get; set; }        
        public string? EmailAddress { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}