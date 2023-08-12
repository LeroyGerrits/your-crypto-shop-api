using DGBCommerce.API.Controllers.Attributes;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DigiByteWalletController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtUtils _jwtUtils;
        private readonly IDigiByteWalletRepository _deliveryMethodRepository;

        public DigiByteWalletController(
            IHttpContextAccessor httpContextAccessor,
            IJwtUtils jwtUtils,
            IDigiByteWalletRepository deliveryMethodRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _jwtUtils = jwtUtils;
            _deliveryMethodRepository = deliveryMethodRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DigiByteWallet>>> Get()
        {
            var deliveryMethods = await _deliveryMethodRepository.Get();
            return Ok(deliveryMethods.ToList());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<DigiByteWallet>> Get(Guid id)
        {
            var deliveryMethod = await _deliveryMethodRepository.GetById(id);
            if (deliveryMethod == null) return NotFound();

            return Ok(deliveryMethod);
        }

        [AuthenticationRequired]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] DigiByteWallet value)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var result = await _deliveryMethodRepository.Create(value, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [AuthenticationRequired]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] DigiByteWallet value)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var deliveryMethod = await _deliveryMethodRepository.GetById(id);
            if (deliveryMethod == null) return NotFound();

            var result = await _deliveryMethodRepository.Update(value, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [AuthenticationRequired]
        [HttpDelete("{id}")]
        public async Task<ActionResult<DigiByteWallet>> Delete(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var deliveryMethod = await _deliveryMethodRepository.GetById(id);
            if (deliveryMethod == null) return NotFound();

            var result = await _deliveryMethodRepository.Delete(id, authenticatedMerchantId.Value);
            return Ok(result);
        }
    }
}