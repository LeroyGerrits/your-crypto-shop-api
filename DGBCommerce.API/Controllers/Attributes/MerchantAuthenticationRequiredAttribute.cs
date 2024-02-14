using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using DGBCommerce.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace DGBCommerce.API.Controllers.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MerchantAuthenticationRequiredAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            var merchant = (Merchant)context.HttpContext.Items["Merchant"]!;
            if (merchant == null)
                context.Result = new JsonResult(new { message = "You are not authorized to use this endpoint" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}