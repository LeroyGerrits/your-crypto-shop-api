using DGBCommerce.API.Controllers.Requests;
using DGBCommerce.API.Controllers.Responses;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;

namespace DGBCommerce.API.Services
{
    public interface IAuthenticationService
    {
        AuthenticationResponse? Authenticate(AuthenticateRequest model);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMerchantRepository _merchantRepository;
        private readonly IJwtUtils _jwtUtils;

        public AuthenticationService(
            IHttpContextAccessor httpContextAccessor,
            IMerchantRepository merchantRepository, 
            IJwtUtils jwtUtils)
        {
            _httpContextAccessor = httpContextAccessor;
            _merchantRepository = merchantRepository;
            _jwtUtils = jwtUtils;
        }

        public AuthenticationResponse? Authenticate(AuthenticateRequest model)
        {
            Merchant? merchant = _merchantRepository.GetByEmailAddressAndPassword(model.EmailAddress, model.Password, _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString()).Result;
            if (merchant == null) return null;
            var token = _jwtUtils.GenerateJwtToken(merchant);
            return new AuthenticationResponse(merchant, token);
        }
    }
}