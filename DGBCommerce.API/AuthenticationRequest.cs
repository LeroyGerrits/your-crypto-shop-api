namespace DGBCommerce.API
{
    public class AuthenticationRequest
    {
        public required string EmailAddress { get; set; }
        public required string Password { get; set; }
    }
}
