namespace YourCryptoShop.API.Controllers.Requests
{
    public class ActivateAccountRequest
    {
        public required Guid Id { get; set; }
        public required string CurrentPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}