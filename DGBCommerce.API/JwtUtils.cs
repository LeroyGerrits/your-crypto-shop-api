using DGBCommerce.Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DGBCommerce.API
{
    public interface IJwtUtils
    {
        public string GenerateJwtToken(Customer customer);
        public string GenerateJwtToken(Merchant merchant);
        public JwtSecurityToken? ValidateJwtToken(string? token);
        public Guid? GetCustomerId(IHttpContextAccessor httpContextAccessor);
        public Guid? GetMerchantId(IHttpContextAccessor httpContextAccessor);
    }

    public class JwtUtils : IJwtUtils
    {
        private readonly AppSettings _appSettings;

        public JwtUtils(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;

            if (string.IsNullOrEmpty(_appSettings.Secret))
                throw new Exception("JWT secret not configured.");
        }

        public string GenerateJwtToken(Customer customer)
            => GenerateJwtToken(customer.Id.ToString()!, nameof(Customer));

        public string GenerateJwtToken(Merchant merchant)
            => GenerateJwtToken(merchant.Id.ToString()!, nameof(Merchant));

        public JwtSecurityToken? ValidateJwtToken(string? token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret!);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                return jwtToken;
            }
            catch
            {
                return null;
            }
        }

        public Guid? GetCustomerId(IHttpContextAccessor httpContextAccessor)
            => GetId(httpContextAccessor, nameof(Customer));

        public Guid? GetMerchantId(IHttpContextAccessor httpContextAccessor)
            => GetId(httpContextAccessor, nameof(Merchant));

        private Guid? GetId(IHttpContextAccessor httpContextAccessor, string type)
        {
            var authorizationToken = httpContextAccessor.HttpContext?.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            var jwtToken = ValidateJwtToken(authorizationToken);
            if (jwtToken != null)
            {
                var tokenId = new Guid(jwtToken.Claims.First(x => x.Type == "id").Value);
                var tokenType = jwtToken.Claims.First(x => x.Type == "type").Value;
                return tokenType == type ? tokenId : null;
            }
            else
            {
                return null;
            }
        }

        private string GenerateJwtToken(string id, string type)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("id", id),
                    new Claim("type", type)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}