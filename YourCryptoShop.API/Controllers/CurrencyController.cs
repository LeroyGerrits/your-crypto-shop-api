using YourCryptoShop.Domain.Interfaces.Repositories;
using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourCryptoShop.Domain.Enums;

namespace YourCryptoShop.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController(ICurrencyRepository currencyRepository) : ControllerBase
    {
        private readonly ICurrencyRepository _currencyRepository = currencyRepository;

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Currency>>> Get(CurrencyType? type, string? symbol, string? code, string? name, bool? supported)
        {
            var currencies = await _currencyRepository.Get(new GetCurrenciesParameters()
            {
                Type = type,
                Symbol = symbol,
                Code = code,
                Name = name,
                Supported = supported
            });
            return Ok(currencies.ToList());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Currency>> GetById(Guid id)
        {
            var currency = await _currencyRepository.GetById(id);
            if (currency == null)
                return NotFound();

            return Ok(currency);
        }
    }
}