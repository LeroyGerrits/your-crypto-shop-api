using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatsController(IStatsRepository currencyRepository) : ControllerBase
    {
        private readonly IStatsRepository _statsRepository = currencyRepository;

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<Stats>> Get()
        {
            var stats = await _statsRepository.Get(null!);
            return Ok(stats.SingleOrDefault());
        }
    }
}