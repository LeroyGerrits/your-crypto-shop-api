using DGBCommerce.Domain.Interfaces.Repositories;

namespace DGBCommerce.API
{
    public class JwtMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context, IMerchantRepository merchantRepository, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            var merchantId = jwtUtils.ValidateJwtToken(token);
            if (merchantId != null)
            {
                var merchant = Task.Run(() => merchantRepository.GetById(merchantId.Value, merchantId.Value)).Result;
                context.Items["Merchant"] = merchant;
            }

            await _next(context);
        }
    }
}