using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _currencyRepository;

        public CountryController(ICountryRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> Get(string? name)
        {
            var currencies = await _currencyRepository.Get(new GetCountriesParameters() { Name = name });
            return Ok(currencies.ToList());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> GetById(Guid id)
        {
            var currency = await _currencyRepository.GetById(id);
            if (currency == null) 
                return NotFound();

            return Ok(currency);
        }
    }
}