using YourCryptoShop.API.Controllers.Requests;
using YourCryptoShop.API.Controllers.Responses;
using YourCryptoShop.Domain.Interfaces.Repositories;
using YourCryptoShop.Domain.Models;

namespace YourCryptoShop.API.Services
{
    public interface IAuthenticationService
    {
        AuthenticateCustomerResponse? AuthenticateCustomer(AuthenticateCustomerRequest model);
        AuthenticateMerchantResponse? AuthenticateMerchant(AuthenticateRequest model);
    }

    public class AuthenticationService(IHttpContextAccessor httpContextAccessor, ICustomerRepository customerRepository, IMerchantRepository merchantRepository, IJwtUtils jwtUtils) : IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ICustomerRepository _customerRepository = customerRepository;
        private readonly IMerchantRepository _merchantRepository = merchantRepository;
        private readonly IJwtUtils _jwtUtils = jwtUtils;

        public AuthenticateCustomerResponse? AuthenticateCustomer(AuthenticateCustomerRequest request)
        {
            Customer? customer = _customerRepository.GetByEmailAddressAndPassword(request.ShopId, request.EmailAddress, request.Password, _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString()).Result;
            if (customer == null) return null;
            var token = _jwtUtils.GenerateJwtToken(customer);
            return new AuthenticateCustomerResponse(customer, token);
        }

        public AuthenticateMerchantResponse? AuthenticateMerchant(AuthenticateRequest request)
        {
            Merchant? merchant = _merchantRepository.GetByEmailAddressAndPassword(request.EmailAddress, request.Password, _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString()).Result;
            if (merchant == null) return null;
            var token = _jwtUtils.GenerateJwtToken(merchant);
            return new AuthenticateMerchantResponse(merchant, token);
        }
    }
}