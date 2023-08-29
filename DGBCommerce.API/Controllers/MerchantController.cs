using DGBCommerce.API.Controllers.Attributes;
using DGBCommerce.API.Controllers.Requests;
using DGBCommerce.API.Services;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DGBCommerce.Domain;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MerchantController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtUtils _jwtUtils;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMerchantRepository _merchantRepository;
        private readonly IMerchantPasswordResetLinkRepository _merchantPasswordResetLinkRepository;

        public MerchantController(
            IHttpContextAccessor httpContextAccessor,
            IJwtUtils jwtUtils,
            IAuthenticationService authenticationService,
            IMerchantRepository merchantRepository,
            IMerchantPasswordResetLinkRepository merchantPasswordResetLinkRepository
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _jwtUtils = jwtUtils;
            _authenticationService = authenticationService;
            _merchantRepository = merchantRepository;
            _merchantPasswordResetLinkRepository = merchantPasswordResetLinkRepository;
        }

        [AuthenticationRequired]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Merchant>>> Get()
        {
            var merchants = await _merchantRepository.Get();
            return Ok(merchants.ToList());
        }

        [AuthenticationRequired]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Merchant value)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var result = await _merchantRepository.Create(value, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [AuthenticationRequired]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] Merchant value)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var merchant = await _merchantRepository.GetById(id);
            if (merchant == null) return NotFound();

            var result = await _merchantRepository.Update(value, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [AuthenticationRequired]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Merchant>> Delete(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var merchant = await _merchantRepository.GetById(id);
            if (merchant == null) return NotFound();

            var result = await _merchantRepository.Delete(id, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate(AuthenticationRequest model)
        {
            var response = _authenticationService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "E-mail address or password is incorrect" });

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<ActionResult> ForgotPassword([FromBody] string emailAddress)
        {
            var merchant = await _merchantRepository.GetByEmailAddress(emailAddress);
            if (merchant != null)
            {
                MerchantPasswordResetLink passwordResetLink = new()
                {
                    Merchant = merchant,
                    Date = DateTime.UtcNow,
                    IpAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress.ToString(),
                    Key = Utilities.GenerateSalt()
                };

                var result = await _merchantPasswordResetLinkRepository.Create(passwordResetLink, Guid.Empty);
                return Ok("boem");
            }

            return Ok();
        }
    }
}