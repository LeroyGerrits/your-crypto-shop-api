using DGBCommerce.Domain.Models;

namespace DGBCommerce.API
{
    public class AuthenticationResponse
    {
        public Merchant Merchant { get; set; }
        public string Token { get; set; }

        public AuthenticationResponse(Merchant merchant, string token)
        {
            Merchant = merchant;
            Token = token;
        }
    }
}