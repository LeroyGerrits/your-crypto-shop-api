using DGBCommerce.API.Controllers.Attributes;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MerchantPasswordResetLinkController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtUtils _jwtUtils;
        private readonly IMerchantPasswordResetLinkRepository _merchantPasswordResetLinkRepository;

        public MerchantPasswordResetLinkController(
            IHttpContextAccessor httpContextAccessor,
            IJwtUtils jwtUtils,
            IMerchantPasswordResetLinkRepository merchantPasswordResetLinkRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _jwtUtils = jwtUtils;
            _merchantPasswordResetLinkRepository = merchantPasswordResetLinkRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<DeliveryMethod>> Get(Guid id, string key)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var merchantPasswordResetLink = await _merchantPasswordResetLinkRepository.GetByIdAndKey(id, key);
            if (merchantPasswordResetLink == null)
                return NotFound();

            return Ok(merchantPasswordResetLink);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MerchantPasswordResetLink value)
        {
            var result = await _merchantPasswordResetLinkRepository.Create(value, Guid.Empty);
            return Ok(result);
        }

        [AuthenticationRequired]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] MerchantPasswordResetLink value)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var merchantPasswordResetLink = await _merchantPasswordResetLinkRepository.GetById(authenticatedMerchantId.Value, id);
            if (merchantPasswordResetLink == null)
                return NotFound();

            var result = await _merchantPasswordResetLinkRepository.Update(value, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [AuthenticationRequired]
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeliveryMethod>> Delete(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var merchantPasswordResetLink = await _merchantPasswordResetLinkRepository.GetById(authenticatedMerchantId.Value, id);
            if (merchantPasswordResetLink == null)
                return NotFound();

            var result = await _merchantPasswordResetLinkRepository.Delete(id, authenticatedMerchantId.Value);
            return Ok(result);
        }
    }
}