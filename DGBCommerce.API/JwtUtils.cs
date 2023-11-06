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
        public string GenerateJwtToken(Merchant merchant);
        public Guid? ValidateJwtToken(string? token);
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

        public string GenerateJwtToken(Merchant merchant)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", merchant.Id.ToString()!) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public Guid? ValidateJwtToken(string? token)
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
                return new Guid(jwtToken.Claims.First(x => x.Type == "id").Value);
            }
            catch
            {
                return null;
            }
        }

        public Guid? GetMerchantId(IHttpContextAccessor httpContextAccessor)
        {
            var token = httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token == null) return null;
            return ValidateJwtToken(token);
        }
    }
}