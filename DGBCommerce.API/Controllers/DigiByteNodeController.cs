using DGBCommerce.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DigiByteNodeController : ControllerBase
    {
        private readonly IRpcService _rpcService;
        
        public DigiByteNodeController(
            IRpcService rpcService)
        {
            _rpcService = rpcService;
        }

        [AllowAnonymous]
        [HttpGet("getcurrentblock")]
        public async Task<ActionResult<double>> GetCurrentBlock()
        {
            var currentBlock = await _rpcService.GetCurrentBlock();
            return Ok(currentBlock);
        }

        [AllowAnonymous]
        [HttpGet("gethashrate")]
        public async Task<ActionResult<double>> GetHashrate()
        {
            var hashRate = await _rpcService.GetHashrate();
            return Ok(hashRate);
        }

        [AllowAnonymous]
        [HttpGet("getdifficulty")]
        public async Task<ActionResult<double>> GetDifficulty()
        {
            var difficulty = await _rpcService.GetDifficulty();
            return Ok(difficulty);
        }
    }
}