namespace DGBCommerce.Domain.Parameters
{
    public class GetMerchantForLoginParameters
    {
        public required string EmailAddress { get; set; }
        public required string Password { get; set; }
    }
}