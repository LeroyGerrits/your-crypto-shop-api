namespace DGBCommerce.API.Controllers.Requests
{
    public class AuthenticationRequest
    {
        public required string EmailAddress { get; set; }
        public required string Password { get; set; }
    }
}