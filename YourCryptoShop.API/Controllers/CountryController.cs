using YourCryptoShop.Domain.Interfaces.Repositories;
using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YourCryptoShop.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountryController(ICountryRepository currencyRepository) : ControllerBase
    {
        private readonly ICountryRepository _countryRepository = currencyRepository;

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> Get(string? name)
        {
            var countries = await _countryRepository.Get(new GetCountriesParameters() { Name = name });
            return Ok(countries.ToList());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> GetById(Guid id)
        {
            var country = await _countryRepository.GetById(id);
            if (country == null)
                return NotFound();

            return Ok(country);
        }
    }
}