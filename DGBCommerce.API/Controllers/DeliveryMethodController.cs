using DGBCommerce.API.Controllers.Attributes;
using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeliveryMethodController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtUtils _jwtUtils;
        private readonly IDeliveryMethodRepository _deliveryMethodRepository;

        public DeliveryMethodController(
            ILogger<CategoryController> logger,
            IHttpContextAccessor httpContextAccessor,
            IJwtUtils jwtUtils,
            IDeliveryMethodRepository deliveryMethodRepository)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _jwtUtils = jwtUtils;
            _deliveryMethodRepository = deliveryMethodRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryMethod>>> Get()
        {
            IEnumerable<DeliveryMethod> deliveryMethods = await _deliveryMethodRepository.Get();
            return Ok(deliveryMethods.ToList());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryMethod>> Get(Guid id)
        {
            DeliveryMethod? deliveryMethod = await _deliveryMethodRepository.GetById(id);
            if (deliveryMethod == null) return NotFound();

            return Ok(deliveryMethod);
        }

        [AuthenticationRequired]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] DeliveryMethod value)
        {
            var merchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (merchantId == null)
                return BadRequest("Merchant not authorized.");

            var result = await _deliveryMethodRepository.Create(value, merchantId.Value);
            if (result.ErrorCode == 0)
                return CreatedAtAction(nameof(_deliveryMethodRepository.Create), new { id = result.Identifier });
            else
                return BadRequest(result);
        }

        [AuthenticationRequired]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] DeliveryMethod value)
        {
            var merchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (merchantId == null)
                return BadRequest("Merchant not authorized.");

            DeliveryMethod? deliveryMethod = await _deliveryMethodRepository.GetById(id);
            if (deliveryMethod == null) return NotFound();
            if (deliveryMethod.Shop.Merchant.Id != merchantId) return BadRequest("");

            var result = await _deliveryMethodRepository.Update(value);
            if (result.ErrorCode == 0)
                return Ok(value);
            else
                return BadRequest(result.Message);
        }

        [AuthenticationRequired]
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeliveryMethod>> Delete(Guid id)
        {
            DeliveryMethod? deliveryMethod = await _deliveryMethodRepository.GetById(id);
            if (deliveryMethod == null) return NotFound();

            var result = await _deliveryMethodRepository.Delete(id);
            if (result.ErrorCode == 0)
                return Ok();
            else
                return BadRequest(result.Message);
        }
    }
}