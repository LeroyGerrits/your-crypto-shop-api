using YourCryptoShop.API.Controllers.Attributes;
using YourCryptoShop.API.Controllers.Requests;
using YourCryptoShop.API.Controllers.Responses;
using YourCryptoShop.Data.Repositories;
using YourCryptoShop.Domain.Interfaces.Repositories;
using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Models.ViewModels;
using YourCryptoShop.Domain.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YourCryptoShop.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeliveryMethodController(
        ICountryRepository countryRepository,
        IDeliveryMethodRepository deliveryMethodRepository,
        IDeliveryMethodCostsPerCountryRepository deliveryMethodCostsPerCountryRepository,
        IHttpContextAccessor httpContextAccessor,
        IJwtUtils jwtUtils) : ControllerBase
    {
        [MerchantAuthenticationRequired]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryMethod>>> Get(string? name, Guid? shopId)
        {
            var authenticatedMerchantId = jwtUtils.GetMerchantId(httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var deliveryMethods = await deliveryMethodRepository.Get(new GetDeliveryMethodsParameters()
            {
                MerchantId = authenticatedMerchantId.Value,
                ShopId = shopId,
                Name = name
            });
            return Ok(deliveryMethods.ToList());
        }

        [MerchantAuthenticationRequired]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetDeliveryMethodResponse>> Get(Guid id)
        {
            var authenticatedMerchantId = jwtUtils.GetMerchantId(httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var deliveryMethod = await deliveryMethodRepository.GetById(authenticatedMerchantId.Value, id);
            if (deliveryMethod == null)
                return NotFound();

            var deliveryMethodCostsPerCountries = await deliveryMethodCostsPerCountryRepository.Get(new GetDeliveryMethodCostsPerCountryParameters() { DeliveryMethodId = id });
            var costsPerCountry = deliveryMethodCostsPerCountries.Select(c => new KeyValuePair<Guid, decimal>(c.CountryId, c.Costs)).ToDictionary(c => c.Key, x => x.Value);

            return Ok(new GetDeliveryMethodResponse(deliveryMethod, costsPerCountry));
        }

        [AllowAnonymous]
        [HttpGet("public/GetByShopId/{shopId}")]
        public async Task<ActionResult<PublicDeliveryMethod>> GetByShopIdPublic(Guid shopId)
        {
            var deliveryMethods = await deliveryMethodRepository.GetByShopIdPublic(shopId);

            foreach (var deliveryMethod in deliveryMethods)
            {
                var deliveryMethodCostsPerCountry = await deliveryMethodCostsPerCountryRepository.Get(new GetDeliveryMethodCostsPerCountryParameters() { DeliveryMethodId = deliveryMethod.Id });
                foreach (var deliveryMethodCostPerCountry in deliveryMethodCostsPerCountry)
                {
                    if (deliveryMethod.CostsPerCountry.ContainsKey(deliveryMethodCostPerCountry.CountryId))
                        deliveryMethod.CostsPerCountry[deliveryMethodCostPerCountry.CountryId] = deliveryMethodCostPerCountry.Costs;
                    else
                        deliveryMethod.CostsPerCountry.Add(deliveryMethodCostPerCountry.CountryId, deliveryMethodCostPerCountry.Costs);
                }
            }

            return Ok(deliveryMethods);
        }

        [MerchantAuthenticationRequired]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MutateDeliveryMethodRequest value)
        {
            var authenticatedMerchantId = jwtUtils.GetMerchantId(httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var result = await deliveryMethodRepository.Create(value.DeliveryMethod, authenticatedMerchantId.Value);

            if (result.Success)
                this.ProcessCostsPerCountry(authenticatedMerchantId.Value, result.Identifier, value.CostsPerCountry);

            return Ok(result);
        }

        [MerchantAuthenticationRequired]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] MutateDeliveryMethodRequest value)
        {
            var authenticatedMerchantId = jwtUtils.GetMerchantId(httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var deliveryMethod = await deliveryMethodRepository.GetById(authenticatedMerchantId.Value, id);
            if (deliveryMethod == null)
                return NotFound();

            var result = await deliveryMethodRepository.Update(value.DeliveryMethod, authenticatedMerchantId.Value);

            if (result.Success)
                this.ProcessCostsPerCountry(authenticatedMerchantId.Value, id, value.CostsPerCountry);

            return Ok(result);
        }

        [MerchantAuthenticationRequired]
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeliveryMethod>> Delete(Guid id)
        {
            var authenticatedMerchantId = jwtUtils.GetMerchantId(httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var deliveryMethod = await deliveryMethodRepository.GetById(authenticatedMerchantId.Value, id);
            if (deliveryMethod == null)
                return NotFound();

            var result = await deliveryMethodRepository.Delete(id, authenticatedMerchantId.Value);
            return Ok(result);
        }

        private async void ProcessCostsPerCountry(Guid merchantId, Guid deliveryMethodId, Dictionary<Guid, decimal?> costsPerCountry)
        {
            var countries = await countryRepository.Get(new());

            foreach (var country in countries)
                if (costsPerCountry.TryGetValue(country.Id!.Value, out decimal? value) && value.HasValue)
                    await deliveryMethodCostsPerCountryRepository.Create(new() { DeliveryMethodId = deliveryMethodId, CountryId = country.Id.Value, Costs = value.Value }, merchantId);
                else
                    await deliveryMethodCostsPerCountryRepository.Delete(deliveryMethodId, country.Id.Value, merchantId);
        }
    }
}