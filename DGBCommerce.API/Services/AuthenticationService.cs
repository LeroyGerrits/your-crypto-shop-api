using DGBCommerce.API.Controllers.Requests;
using DGBCommerce.API.Controllers.Responses;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;

namespace DGBCommerce.API.Services
{
    public interface IAuthenticationService
    {
        AuthenticateCustomerResponse? AuthenticateCustomer(AuthenticateRequest model);
        AuthenticateMerchantResponse? AuthenticateMerchant(AuthenticateRequest model);
    }

    public class AuthenticationService(IHttpContextAccessor httpContextAccessor, ICustomerRepository customerRepository, IMerchantRepository merchantRepository, IJwtUtils jwtUtils) : IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ICustomerRepository _customerRepository = customerRepository;
        private readonly IMerchantRepository _merchantRepository = merchantRepository;
        private readonly IJwtUtils _jwtUtils = jwtUtils;

        public AuthenticateCustomerResponse? AuthenticateCustomer(AuthenticateRequest model)
        {
            Customer? customer = _customerRepository.GetByEmailAddressAndPassword(model.EmailAddress, model.Password, _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString()).Result;
            if (customer == null) return null;
            var token = _jwtUtils.GenerateJwtToken(customer);
            return new AuthenticateCustomerResponse(customer, token);
        }

        public AuthenticateMerchantResponse? AuthenticateMerchant(AuthenticateRequest model)
        {
            Merchant? merchant = _merchantRepository.GetByEmailAddressAndPassword(model.EmailAddress, model.Password, _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString()).Result;
            if (merchant == null) return null;
            var token = _jwtUtils.GenerateJwtToken(merchant);
            return new AuthenticateMerchantResponse(merchant, token);
        }
    }
}