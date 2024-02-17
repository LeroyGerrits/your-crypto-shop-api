using DGBCommerce.Domain.Models;

namespace DGBCommerce.API.Controllers.Responses
{
    public class AuthenticateMerchantResponse(Merchant merchant, string token)
    {
        public Merchant Merchant { get; set; } = merchant;
        public string Token { get; set; } = token;
    }
}