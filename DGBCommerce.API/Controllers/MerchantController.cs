using DGBCommerce.API.Controllers.Attributes;
using DGBCommerce.API.Controllers.Requests;
using DGBCommerce.API.Services;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MerchantController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMerchantRepository _merchantRepository;

        public MerchantController(
            IAuthenticationService authenticationService,
            IMerchantRepository merchantRepository
            )
        {
            _authenticationService = authenticationService;
            _merchantRepository = merchantRepository;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticationRequest model)
        {
            var response = _authenticationService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [AuthenticationRequired]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Merchant>>> Get()
        {
            IEnumerable<Merchant> merchants = await _merchantRepository.Get();
            return Ok(merchants.ToList());
        }

        [AuthenticationRequired]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Merchant value)
        {
            var result = await _merchantRepository.Insert(value);
            return CreatedAtAction(nameof(Get), new { id = result.Identifier });
        }

        [AuthenticationRequired]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] Merchant value)
        {
            Merchant? merchant = await _merchantRepository.GetById(id);
            if (merchant == null) return NotFound();

            var result = await _merchantRepository.Update(value);
            if (result.ErrorCode > 0)
                return NoContent();

            return NoContent();
        }

        [AuthenticationRequired]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Merchant>> Delete(Guid id)
        {
            Merchant? merchant = await _merchantRepository.GetById(id);
            if (merchant == null) return NotFound();

            var result = await _merchantRepository.Delete(id);
            if (result.ErrorCode > 0)
                return NoContent();

            return Ok(merchant);
        }
    }
}