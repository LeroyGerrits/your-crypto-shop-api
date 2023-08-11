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
    public class ShopController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtUtils _jwtUtils;
        private readonly IShopRepository _shopRepository;

        public ShopController(
            ILogger<CategoryController> logger,
            IHttpContextAccessor httpContextAccessor,
            IJwtUtils jwtUtils,
            IShopRepository shopRepository)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _jwtUtils = jwtUtils;
            _shopRepository = shopRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shop>>> Get()
        {
            IEnumerable<Shop> shops = await _shopRepository.Get();
            return Ok(shops.ToList());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Shop>> Get(Guid id)
        {
            Shop? shop = await _shopRepository.GetById(id);
            if (shop == null) return NotFound();

            return Ok(shop);
        }

        [AuthenticationRequired]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Shop value)
        {
            var merchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (merchantId == null)
                return BadRequest("Merchant not authorized.");

            var result = await _shopRepository.Create(value, merchantId.Value);
            if (result.ErrorCode == 0)
                return CreatedAtAction(nameof(Get), new { id = result.Identifier });
            else
                return BadRequest(result.Message);
        }

        [AuthenticationRequired]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] Shop value)
        {
            Shop? shop = await _shopRepository.GetById(id);
            if (shop == null) return NotFound();

            var result = await _shopRepository.Update(value);
            if (result.ErrorCode == 0)
                return Ok(value);
            else
                return BadRequest(result.Message);
        }

        [AuthenticationRequired]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Shop>> Delete(Guid id)
        {
            Shop? shop = await _shopRepository.GetById(id);
            if (shop == null) return NotFound();

            var result = await _shopRepository.Delete(id);
            if (result.ErrorCode == 0)
                return Ok(shop);
            else
                return BadRequest(result.Message);
        }
    }
}