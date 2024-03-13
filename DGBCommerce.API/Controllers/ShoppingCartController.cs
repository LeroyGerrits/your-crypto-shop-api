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
    public class ShoppingCartController(IHttpContextAccessor httpContextAccessor, IJwtUtils jwtUtils, IShoppingCartRepository shoppingCartRepository, IShoppingCartItemRepository shoppingCartItemRepository) : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IJwtUtils _jwtUtils = jwtUtils;
        private readonly IShoppingCartRepository _shoppingCartRepository = shoppingCartRepository;
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository = shoppingCartItemRepository;

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
        [HttpGet("public/{sessionId}")]
        public async Task<ActionResult<ShoppingCart>> GetPublic(Guid sessionId)
        {
            var shoppingCart = await _shoppingCartRepository.GetBySession(sessionId);
            if (shoppingCart != null)
            {
                var shoppingCartItems = await _shoppingCartItemRepository.GetByShoppingCartId(shoppingCart.Id!.Value);
                shoppingCart.Items = shoppingCartItems.ToList();
                return Ok(shoppingCart);
            }
            else
            {
                var shoppingCartToCreate = new ShoppingCart()
                {
                    CustomerId = _jwtUtils.GetCustomerId(_httpContextAccessor),
                    LastIpAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                    Session = sessionId
                };
                var result = await _shoppingCartRepository.Create(shoppingCartToCreate, Guid.Empty);

                if (result.Success)
                {
                    shoppingCartToCreate.Id = result.Identifier;
                    return Ok(shoppingCartToCreate);
                }
                else
                {
                    return BadRequest(new { message = result.Message });
                }
            }
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

        [AllowAnonymous]
        [HttpPost("public/AddItem")]
        public async Task<ActionResult> AddItem([FromBody] ShoppingCartItem value)
        {
            var shoppingCart = await _shoppingCartRepository.GetById(value.ShoppingCartId);
            if (shoppingCart == null)
                return BadRequest("Merchant not authorized.");

            var result = await _shoppingCartItemRepository.Create(value, Guid.Empty);
            return Ok(result);
        }
    }
}