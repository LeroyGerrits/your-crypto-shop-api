namespace DGBCommerce.API.Controllers.Requests
{
    public class AuthenticateRequest
    {
        public required string EmailAddress { get; set; }
        public required string Password { get; set; }
    }
}