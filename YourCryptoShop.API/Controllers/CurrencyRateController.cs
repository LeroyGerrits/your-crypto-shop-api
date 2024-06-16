using YourCryptoShop.Domain.Interfaces.Repositories;
using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YourCryptoShop.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyRateController(ICurrencyRateRepository currencyRateRepository) : ControllerBase
    {
        private readonly ICurrencyRateRepository _currencyRateRepository = currencyRateRepository;

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Currency>>> Get(Guid? currencyFromId, Guid? currencyToId)
        {
            var currencyRates = await _currencyRateRepository.Get(new GetCurrencyRatesParameters()
            {
                CurrencyFromId = currencyFromId,
                CurrencyToId = currencyToId
            });
            return Ok(currencyRates.ToList());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<CurrencyRate>> GetById(Guid id)
        {
            var currencyRate = await _currencyRateRepository.GetById(id);
            if (currencyRate == null)
                return NotFound();

            return Ok(currencyRate);
        }
    }
}