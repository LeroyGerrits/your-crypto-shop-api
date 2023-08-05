using DGBCommerce.API.Controllers.Requests;
using DGBCommerce.API.Controllers.Responses;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;

namespace DGBCommerce.API.Services
{
    public interface IAuthenticationService
    {
        AuthenticationResponse? Authenticate(AuthenticationRequest model);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMerchantRepository _merchantRepository;
        private readonly IJwtUtils _jwtUtils;

        public AuthenticationService(IMerchantRepository merchantRepository, IJwtUtils jwtUtils)
        {
            _merchantRepository = merchantRepository;
            _jwtUtils = jwtUtils;
        }

        public AuthenticationResponse? Authenticate(AuthenticationRequest model)
        {
            Merchant? merchant = _merchantRepository.GetByEmailAddressAndPassword(model.EmailAddress, model.Password).Result;
            if (merchant == null) return null;
            var token = _jwtUtils.GenerateJwtToken(merchant);
            return new AuthenticationResponse(merchant, token);
        }
    }
}