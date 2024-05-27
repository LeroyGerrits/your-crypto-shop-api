using YourCryptoShop.Domain.Models;

namespace YourCryptoShop.API.Controllers.Responses
{
    public class AuthenticateCustomerResponse(Customer customer, string token)
    {
        public Customer Customer { get; set; } = customer;
        public string Token { get; set; } = token;
    }
}