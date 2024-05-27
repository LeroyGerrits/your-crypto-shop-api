namespace YourCryptoShop.API.Controllers.Requests
{
    public class ResetPasswordRequest
    {
        public required Guid Id { get; set; }
        public required string Key { get; set; }
        public required string Password { get; set; }
    }
}