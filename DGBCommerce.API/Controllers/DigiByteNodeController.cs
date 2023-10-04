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
        [HttpGet("getblockcount")]
        public async Task<ActionResult<double>> GetBlockCount()
        {
            var currentBlock = await _rpcService.GetBlockCount();
            return Ok(currentBlock);
        }

        [AllowAnonymous]
        [HttpGet("getmininginfo")]
        public async Task<ActionResult<double>> GetMiningInfo()
        {
            var hashRate = await _rpcService.GetMiningInfo();
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