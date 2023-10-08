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
    public class ProductController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtUtils _jwtUtils;
        private readonly IProductRepository _deliveryMethodRepository;

        public ProductController(
            IHttpContextAccessor httpContextAccessor,
            IJwtUtils jwtUtils,
            IProductRepository deliveryMethodRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _jwtUtils = jwtUtils;
            _deliveryMethodRepository = deliveryMethodRepository;
        }

        [AuthenticationRequired]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get(string? name, Guid? shopId, Guid? categoryId)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var deliveryMethods = await _deliveryMethodRepository.Get(new GetProductsParameters() { 
                MerchantId = authenticatedMerchantId.Value, 
                Name = name,
                ShopId = shopId,
                CategoryId = categoryId
            });
            return Ok(deliveryMethods.ToList());
        }

        [AuthenticationRequired]
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var deliveryMethod = await _deliveryMethodRepository.GetById(authenticatedMerchantId.Value, id);
            if (deliveryMethod == null)
                return NotFound();

            return Ok(deliveryMethod);
        }

        [AuthenticationRequired]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Product value)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var result = await _deliveryMethodRepository.Create(value, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [AuthenticationRequired]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] Product value)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var deliveryMethod = await _deliveryMethodRepository.GetById(authenticatedMerchantId.Value, id);
            if (deliveryMethod == null)
                return NotFound();

            var result = await _deliveryMethodRepository.Update(value, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [AuthenticationRequired]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> Delete(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var deliveryMethod = await _deliveryMethodRepository.GetById(authenticatedMerchantId.Value, id);
            if (deliveryMethod == null)
                return NotFound();

            var result = await _deliveryMethodRepository.Delete(id, authenticatedMerchantId.Value);
            return Ok(result);
        }
    }
}