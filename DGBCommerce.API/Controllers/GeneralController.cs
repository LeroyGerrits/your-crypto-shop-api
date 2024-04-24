using DGBCommerce.API.Controllers.Attributes;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeneralController(
        IGeneralRepository generalRepository,
        IHttpContextAccessor httpContextAccessor,
        IJwtUtils jwtUtils
        ) : ControllerBase
    {
        [MerchantAuthenticationRequired]
        [HttpGet("GetDashboardSales/{mode}")]
        public async Task<ActionResult<Dictionary<string, decimal>>> Get(string mode)
        {
            var authenticatedMerchantId = jwtUtils.GetMerchantId(httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var sales = await generalRepository.GetDashboardSales(authenticatedMerchantId.Value, mode);
            return Ok(sales);
        }

        [AllowAnonymous]
        [HttpGet("GetStats")]
        public async Task<ActionResult<Stats>> GetStats()
        {
            var stats = await generalRepository.GetStats();
            return Ok(stats);
        }
    }
}