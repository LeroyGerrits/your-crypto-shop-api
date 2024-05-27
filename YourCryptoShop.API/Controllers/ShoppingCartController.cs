using YourCryptoShop.API.Controllers.Attributes;
using YourCryptoShop.API.Controllers.Requests;
using YourCryptoShop.Domain;
using YourCryptoShop.Domain.Interfaces.Repositories;
using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YourCryptoShop.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShoppingCartController(
        IFieldRepository fieldRepository,
        IHttpContextAccessor httpContextAccessor,
        IJwtUtils jwtUtils,
        IShoppingCartRepository shoppingCartRepository,
        IShoppingCartItemRepository shoppingCartItemRepository,
        IShoppingCartItemFieldDataRepository shoppingCartItemFieldDataRepository
        ) : ControllerBase
    {
        [MerchantAuthenticationRequired]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingCart>>> Get(Guid? customerId)
        {
            var authenticatedMerchantId = jwtUtils.GetMerchantId(httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var shoppingCarts = await shoppingCartRepository.Get(new GetShoppingCartsParameters()
            {
                CustomerId = customerId
            });

            return Ok(shoppingCarts.ToList());
        }

        [AllowAnonymous]
        [HttpGet("public/{sessionId}")]
        public async Task<ActionResult<ShoppingCart>> GetPublic(Guid sessionId)
        {
            var shoppingCart = await shoppingCartRepository.GetBySession(sessionId);
            if (shoppingCart != null)
            {
                var shoppingCartItems = await shoppingCartItemRepository.GetByShoppingCartId(shoppingCart.Id!.Value);
                shoppingCart.Items = shoppingCartItems.ToList();
                return Ok(shoppingCart);
            }
            else
            {
                var shoppingCartToCreate = new ShoppingCart()
                {
                    CustomerId = jwtUtils.GetCustomerId(httpContextAccessor),
                    LastIpAddress = httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                    Session = sessionId
                };
                var result = await shoppingCartRepository.Create(shoppingCartToCreate, Guid.Empty);

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
            var authenticatedMerchantId = jwtUtils.GetMerchantId(httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var shoppingCart = await shoppingCartRepository.GetById(authenticatedMerchantId.Value, id);
            if (shoppingCart == null)
                return NotFound();

            return Ok(shoppingCart);
        }

        [AllowAnonymous]
        [HttpPost("public/AddItem")]
        public async Task<ActionResult> AddItem([FromBody] MutateShoppingCartItemRequest value)
        {
            var shoppingCart = await shoppingCartRepository.GetById(value.ShoppingCartItem.ShoppingCartId);
            if (shoppingCart == null)
                return BadRequest("Shopping cart not available.");

            var result = await shoppingCartItemRepository.Create(value.ShoppingCartItem, Guid.Empty);

            if (result.Success)
            {
                this.ProcessFieldData(value.ShopId, result.Identifier, value.FieldData);
            }

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPut("public/EditItem/{id}")]
        public async Task<ActionResult> EditItem(Guid id, [FromBody] MutateShoppingCartItemRequest value)
        {
            var shoppingCart = await shoppingCartRepository.GetById(value.ShoppingCartItem.ShoppingCartId);
            if (shoppingCart == null)
                return BadRequest("Shopping cart not available.");

            var shoppingCartItem = await shoppingCartItemRepository.GetById(id);
            if (shoppingCartItem == null)
                return BadRequest("Shopping cart item not found.");

            var result = await shoppingCartItemRepository.Update(value.ShoppingCartItem, Guid.Empty);

            if (result.Success)
            {
                this.ProcessFieldData(value.ShopId, result.Identifier, value.FieldData);
            }

            return Ok(result);
        }

        [HttpDelete("public/DeleteItem/{sessionId}/{id}")]
        public async Task<ActionResult<Shop>> DeleteItem(Guid sessionId, Guid id)
        {
            var shoppingCart = await shoppingCartRepository.GetBySession(sessionId);
            if (shoppingCart == null)
                return BadRequest("Shopping cart not available.");

            var result = await shoppingCartItemRepository.Delete(id, Guid.Empty);
            return Ok(result);
        }

        private async void ProcessFieldData(Guid shopId, Guid shoppingCartItemId, Dictionary<Guid, string?>? fieldData)
        {
            if (fieldData == null)
                return;

            var fields = await fieldRepository.Get(new GetFieldsParameters() { ShopId = shopId, Entity = Domain.Enums.FieldEntity.Product, Type = Domain.Enums.FieldType.CustomerDefined });
            foreach (Field field in fields)
            {
                string? data = null;

                if (fieldData.TryGetValue(field.Id!.Value, out string? value) && !string.IsNullOrEmpty(value))
                {
                    switch (field.DataType)
                    {
                        case Domain.Enums.FieldDataType.Date:
                            var dataDate = Utilities.NullableDateTime(value);
                            if (dataDate != null)
                                data = dataDate.Value.ToString("yyyy-MM-dd");
                            break;
                        default:
                            data = value;
                            break;
                    }
                }

                if (data != null)
                    await shoppingCartItemFieldDataRepository.Create(new() { ShoppingCartItemId = shoppingCartItemId, FieldId = field.Id!.Value, Data = data }, Guid.Empty);
                else
                    await shoppingCartItemFieldDataRepository.Delete(shoppingCartItemId, field.Id!.Value);
            }
        }
    }
}