using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using DGBCommerce.Domain.Models;

namespace DGGCommerce.API
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthenticationAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (Merchant)context.HttpContext.Items["Merchant"]!;
            if (user == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized (XXX)" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}