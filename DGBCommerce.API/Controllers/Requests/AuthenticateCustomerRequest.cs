namespace DGBCommerce.API.Controllers.Requests
{
    public class AuthenticateCustomerRequest
    {
        public required Guid ShopId { get; set; }
        public required string EmailAddress { get; set; }
        public required string Password { get; set; }
    }
}