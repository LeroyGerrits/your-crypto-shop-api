using YourCryptoShop.Domain.Models;

namespace YourCryptoShop.API.Controllers.Responses
{
    public class AuthenticateMerchantResponse(Merchant merchant, string token)
    {
        public Merchant Merchant { get; set; } = merchant;
        public string Token { get; set; } = token;
    }
}