using DGBCommerce.API.Controllers.Attributes;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShoppingCartController(IHttpContextAccessor httpContextAccessor, IJwtUtils jwtUtils, IShoppingCartRepository shoppingCartRepository) : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IJwtUtils _jwtUtils = jwtUtils;
        private readonly IShoppingCartRepository _shoppingCartRepository = shoppingCartRepository;

        [MerchantAuthenticationRequired]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingCart>>> Get(Guid? customerId)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var shoppingCarts = await _shoppingCartRepository.Get(new GetShoppingCartsParameters()
            {
                CustomerId = customerId
            });

            return Ok(shoppingCarts.ToList());
        }

        [AllowAnonymous]
        [HttpGet("public")]
        public async Task<ActionResult<ShoppingCart>> GetPublic()
        {
            var sessionId = GetSessionId();
            var shoppingCart = await _shoppingCartRepository.GetBySession(sessionId);
            return Ok(shoppingCart);
        }

        [MerchantAuthenticationRequired]
        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingCart>> GetById(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var shoppingCart = await _shoppingCartRepository.GetById(authenticatedMerchantId.Value, id);
            if (shoppingCart == null)
                return NotFound();

            return Ok(shoppingCart);
        }

        private Guid GetSessionId()
        {
            if (_httpContextAccessor.HttpContext == null)
                throw new Exception("No HTTP context available.");

            if (_httpContextAccessor.HttpContext.Items["shoppingCartSessionId"] != null && Guid.TryParse(_httpContextAccessor.HttpContext.Items["shoppingCartSessionId"]!.ToString(), out Guid sessionIdContext))
                return sessionIdContext;

            Guid sessionId = Guid.NewGuid();
            _httpContextAccessor.HttpContext!.Items["shoppingCartSessionId"] = sessionId;

            return sessionId;
        }
    }
}