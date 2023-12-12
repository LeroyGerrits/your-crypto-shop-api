using DGBCommerce.API.Controllers.Attributes;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using Microsoft.AspNetCore.Mvc;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeliveryMethodController(IHttpContextAccessor httpContextAccessor, IJwtUtils jwtUtils, IDeliveryMethodRepository deliveryMethodRepository) : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IJwtUtils _jwtUtils = jwtUtils;
        private readonly IDeliveryMethodRepository _deliveryMethodRepository = deliveryMethodRepository;

        [AuthenticationRequired]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryMethod>>> Get(string? name, Guid? shopId)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var deliveryMethods = await _deliveryMethodRepository.Get(new GetDeliveryMethodsParameters()
            {
                MerchantId = authenticatedMerchantId.Value,
                ShopId = shopId,
                Name = name
            });
            return Ok(deliveryMethods.ToList());
        }

        [AuthenticationRequired]
        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryMethod>> Get(Guid id)
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
        public async Task<ActionResult> Post([FromBody] DeliveryMethod value)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var result = await _deliveryMethodRepository.Create(value, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [AuthenticationRequired]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] DeliveryMethod value)
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
        public async Task<ActionResult<DeliveryMethod>> Delete(Guid id)
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