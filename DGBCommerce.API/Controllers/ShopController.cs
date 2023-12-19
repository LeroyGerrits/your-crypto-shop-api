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
    public class ShopController(IOptions<AppSettings> appSettings, IHttpContextAccessor httpContextAccessor, IJwtUtils jwtUtils, IShopRepository shopRepository) : ControllerBase
    {
        private readonly AppSettings _appSettings = appSettings.Value;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IJwtUtils _jwtUtils = jwtUtils;
        private readonly IShopRepository _shopRepository = shopRepository;

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
                return BadRequest(new { message = "Sub domain is required." });

            if (!string.IsNullOrWhiteSpace(_appSettings.ReservedSubDomains) && _appSettings.ReservedSubDomains.Contains(subDomain, StringComparison.CurrentCultureIgnoreCase))
                return false;

            Guid? idGuid = Utilities.NullableGuid(id ?? string.Empty);
            var shop = await _shopRepository.GetByIdAndSubDomainPublic(idGuid, subDomain);
            return Ok(shop == null);
        }

        [AuthenticationRequired]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shop>>> Get(string? name, string? subDomain, Guid? countryId, Guid? categoryId)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var shops = await _shopRepository.Get(new GetShopsParameters()
            {
                MerchantId = authenticatedMerchantId.Value,
                Name = name,
                SubDomain = subDomain,
                CountryId = countryId,
                CategoryId = categoryId
            });
            return Ok(shops.ToList());
        }

        [AllowAnonymous]
        [HttpGet("public")]
        public async Task<ActionResult<IEnumerable<PublicShop>>> GetPublic(string? name, string? subDomain, Guid? countryId, Guid? categoryId)
        {
            var shops = await _shopRepository.GetPublic(new GetShopsParameters()
            {
                Name = name,
                SubDomain = subDomain,
                CountryId = countryId,
                CategoryId = categoryId
            });
            return Ok(shops.ToList());
        }

        [AuthenticationRequired]
        [HttpGet("{id}")]
        public async Task<ActionResult<Shop>> GetById(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var shop = await _shopRepository.GetById(authenticatedMerchantId.Value, id);
            if (shop == null)
                return NotFound();

            return Ok(shop);
        }

        [AllowAnonymous]
        [HttpGet("public/{id}")]
        public async Task<ActionResult<PublicShop>> GetByIdPublic(Guid id)
        {
            var shop = await _shopRepository.GetByIdPublic(id);
            if (shop == null)
                return NotFound();

            return Ok(shop);
        }

        [AllowAnonymous]
        [HttpGet("public/GetBySubDomain/{subDomain}")]
        public async Task<ActionResult<PublicShop>> GetBySubDomainPublic(string subDomain)
        {
            var shop = await _shopRepository.GetBySubDomainPublic(subDomain);
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

            if (!string.IsNullOrWhiteSpace(value.SubDomain))
            {
                if (!string.IsNullOrWhiteSpace(_appSettings.ReservedSubDomains) && _appSettings.ReservedSubDomains.Contains(value.SubDomain, StringComparison.CurrentCultureIgnoreCase))
                    return BadRequest(new { message = "This subdomain is already taken." });

                var merchantHasShopsWithSubDomains = await this.MerchantHasShopsWithSubDomains(authenticatedMerchantId.Value, null);
                if (merchantHasShopsWithSubDomains)
                    return BadRequest(new { message = "At this time, you can only claim one sub domain per account." });
            }

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

            if (!string.IsNullOrWhiteSpace(value.SubDomain))
            {
                if (!string.IsNullOrWhiteSpace(_appSettings.ReservedSubDomains) && _appSettings.ReservedSubDomains.Contains(value.SubDomain, StringComparison.CurrentCultureIgnoreCase))
                    return BadRequest(new { message = "This subdomain is already taken." });

                var merchantHasShopsWithSubDomains = await this.MerchantHasShopsWithSubDomains(authenticatedMerchantId.Value, id);
                if (merchantHasShopsWithSubDomains)
                    return BadRequest(new { message = "At this time, you can only claim one sub domain per account." });
            }

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

        private async Task<bool> MerchantHasShopsWithSubDomains(Guid merchantId, Guid? shopId)
        {
            var shops = await _shopRepository.Get(new GetShopsParameters() { MerchantId = merchantId });
            var shopsWithSubdomains = shops.Where(s => !string.IsNullOrWhiteSpace(s.SubDomain) && (!shopId.HasValue || (shopId.HasValue && s.Id != shopId)));
            return shopsWithSubdomains.Any();
        }
    }
}