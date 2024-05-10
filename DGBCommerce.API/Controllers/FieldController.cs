using DGBCommerce.API.Controllers.Attributes;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Models.ViewModels;
using DGBCommerce.Domain.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FieldController(
        IFieldRepository fieldRepository,
        IHttpContextAccessor httpContextAccessor,
        IJwtUtils jwtUtils) : ControllerBase
    {
        [MerchantAuthenticationRequired]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Field>>> Get(string? name, Guid? shopId)
        {
            var authenticatedMerchantId = jwtUtils.GetMerchantId(httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var fields = await fieldRepository.Get(new GetFieldsParameters()
            {
                MerchantId = authenticatedMerchantId.Value,
                ShopId = shopId,
                Name = name
            });

            return Ok(fields.ToList());
        }

        [MerchantAuthenticationRequired]
        [HttpGet("{id}")]
        public async Task<ActionResult<Field>> Get(Guid id)
        {
            var authenticatedMerchantId = jwtUtils.GetMerchantId(httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var field = await fieldRepository.GetById(authenticatedMerchantId.Value, id);
            if (field == null)
                return NotFound();

            return Ok(field);
        }

        [AllowAnonymous]
        [HttpGet("public/GetByShopId/{shopId}")]
        public async Task<ActionResult<PublicField>> GetByShopIdPublic(Guid shopId)
        {
            var fields = await fieldRepository.GetByShopIdPublic(shopId);
            return Ok(fields);
        }

        [MerchantAuthenticationRequired]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Field value)
        {
            var authenticatedMerchantId = jwtUtils.GetMerchantId(httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var result = await fieldRepository.Create(value, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [MerchantAuthenticationRequired]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] Field value)
        {
            var authenticatedMerchantId = jwtUtils.GetMerchantId(httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var field = await fieldRepository.GetById(authenticatedMerchantId.Value, id);
            if (field == null)
                return NotFound();

            var result = await fieldRepository.Update(value, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [MerchantAuthenticationRequired]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Field>> Delete(Guid id)
        {
            var authenticatedMerchantId = jwtUtils.GetMerchantId(httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var field = await fieldRepository.GetById(authenticatedMerchantId.Value, id);
            if (field == null)
                return NotFound();

            var result = await fieldRepository.Delete(id, authenticatedMerchantId.Value);
            return Ok(result);
        }
    }
}