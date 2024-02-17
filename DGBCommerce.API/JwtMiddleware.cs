using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Interfaces.Repositories;

namespace DGBCommerce.API
{
    public class JwtMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context, ICustomerRepository customerRepository, IMerchantRepository merchantRepository, IJwtUtils jwtUtils)
        {
            var authorizationToken = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            var jwtToken = jwtUtils.ValidateJwtToken(authorizationToken);
            if (jwtToken != null)
            {
                var id = new Guid(jwtToken.Claims.First(x => x.Type == "id").Value);
                var type = jwtToken.Claims.First(x => x.Type == "type").Value;

                if (type == nameof(Customer))
                {
                    var merchant = Task.Run(() => customerRepository.GetById(id, id)).Result;
                    context.Items["Customer"] = merchant;
                }

                if (type == nameof(Merchant))
                {
                    var merchant = Task.Run(() => merchantRepository.GetById(id, id)).Result;
                    context.Items["Merchant"] = merchant;
                }
            }

            await _next(context);
        }
    }
}