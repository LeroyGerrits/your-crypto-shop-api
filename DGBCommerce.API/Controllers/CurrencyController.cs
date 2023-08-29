using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyController(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Currency>>> Get()
        {
            var currencies = await _currencyRepository.Get();
            return Ok(currencies.ToList());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Currency>> Get(Guid id)
        {
            var currency = await _currencyRepository.GetById(id);
            if (currency == null) return NotFound();

            return Ok(currency);
        }
    }
}