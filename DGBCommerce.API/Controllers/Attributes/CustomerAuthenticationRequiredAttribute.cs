using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using DGBCommerce.Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace DGBCommerce.API.Controllers.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomerAuthenticationRequiredAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            var customer = (Customer)context.HttpContext.Items["Customer"]!;
            if (customer == null)
                context.Result = new JsonResult(new { message = "You are not authorized to use this endpoint" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}