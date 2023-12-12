using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController(ICurrencyRepository currencyRepository) : ControllerBase
    {
        private readonly ICurrencyRepository _currencyRepository = currencyRepository;

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Currency>>> Get(string? name)
        {
            var currencies = await _currencyRepository.Get(new GetCurrenciesParameters() { Name = name });
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