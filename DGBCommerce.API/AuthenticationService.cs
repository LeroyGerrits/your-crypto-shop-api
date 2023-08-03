using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DGBCommerce.API
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse?> Authenticate(AuthenticationRequest model);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly AppSettings _appSettings;
        private readonly IMerchantRepository _merchantRepository;

        public AuthenticationService(
            IOptions<AppSettings> appSettings,
            IMerchantRepository merchantRepository)
        {
            _appSettings = appSettings.Value;
            _merchantRepository = merchantRepository;
        }

        public async Task<AuthenticationResponse?> Authenticate(AuthenticationRequest model)
        {
            var merchant = await _merchantRepository.GetByEmailAddressAndPassword(model.EmailAddress, model.Password);
            if (merchant == null) return null;
            var token = GenerateJwtToken(merchant);
            return new AuthenticationResponse(merchant, token);
        }

        private string GenerateJwtToken(Merchant merchant)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret!);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", merchant.Id.ToString()!) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature
                )
            });
            return tokenHandler.WriteToken(token);
        }
    }
}
