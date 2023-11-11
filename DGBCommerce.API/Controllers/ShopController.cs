using DGBCommerce.API.Controllers.Attributes;
using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Models.ViewModels;
using DGBCommerce.Domain.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShopController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtUtils _jwtUtils;
        private readonly IShopRepository _shopRepository;

        public ShopController(
            IOptions<AppSettings> appSettings,
            IHttpContextAccessor httpContextAccessor,
            IJwtUtils jwtUtils,
            IShopRepository shopRepository)
        {
            _appSettings = appSettings.Value;
            _httpContextAccessor = httpContextAccessor;
            _jwtUtils = jwtUtils;
            _shopRepository = shopRepository;
        }

        [AllowAnonymous]
        [HttpGet("public")]
        public async Task<ActionResult<IEnumerable<PublicShop>>> GetPublic(string? name, string? subDomain)
        {
            var shops = await _shopRepository.GetPublic(new GetShopsParameters() { Name = name, SubDomain = subDomain });
            return Ok(shops.ToList());
        }

        [AllowAnonymous]
        [HttpGet("public/featured")]
        public async Task<ActionResult<IEnumerable<PublicShop>>> GetFeaturedPublic()
        {
            var shops = await _shopRepository.GetPublic(new GetShopsParameters() { Featured = true });
            return Ok(shops.ToList());
        }

        [AllowAnonymous]
        [HttpGet("public/subdomain-available")]
        public async Task<ActionResult<bool>> GetSubDomainAvailablePublic(string subDomain, string? id)
        {
            if (string.IsNullOrWhiteSpace(subDomain))
                return BadRequest(new { Message = "Sub domain is required." });

            if (!string.IsNullOrWhiteSpace(_appSettings.ReservedSubDomains) && _appSettings.ReservedSubDomains.ToLower().Contains(subDomain.ToLower()))
                return false;

            Guid? idGuid = Utilities.NullableGuid(id ?? string.Empty);
            var shop = await _shopRepository.GetByIdAndSubDomainPublic(idGuid, subDomain);
            return Ok(shop == null);
        }

        [AuthenticationRequired]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shop>>> Get(string? name, string? subDomain)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var shops = await _shopRepository.Get(new GetShopsParameters()
            {
                MerchantId = authenticatedMerchantId.Value,
                Name = name,
                SubDomain = subDomain
            });
            return Ok(shops.ToList());
        }

        [AuthenticationRequired]
        [HttpGet("{id}")]
        public async Task<ActionResult<Shop>> Get(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var shop = await _shopRepository.GetById(authenticatedMerchantId.Value, id);
            if (shop == null)
                return NotFound();

            return Ok(shop);
        }

        [AuthenticationRequired]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Shop value)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var result = await _shopRepository.Create(value, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [AuthenticationRequired]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] Shop value)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var shop = await _shopRepository.GetById(authenticatedMerchantId.Value, id);
            if (shop == null)
                return NotFound();

            var result = await _shopRepository.Update(value, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [AuthenticationRequired]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Shop>> Delete(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var shop = await _shopRepository.GetById(authenticatedMerchantId.Value, id);
            if (shop == null)
                return NotFound();

            var result = await _shopRepository.Delete(id, authenticatedMerchantId.Value);
            return Ok(result);
        }
    }
}