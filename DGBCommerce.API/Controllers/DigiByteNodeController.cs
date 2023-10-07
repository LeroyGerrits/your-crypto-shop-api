using DGBCommerce.Domain.Interfaces.Services;
using DGBCommerce.Domain.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DigiByteNodeController : ControllerBase
    {
        private readonly IRpcService _rpcService;
        
        public DigiByteNodeController(IRpcService rpcService)
        {
            _rpcService = rpcService;
        }

        [AllowAnonymous]
        [HttpGet("getblockcount")]
        public async Task<ActionResult<uint>> GetBlockCount()
        {
            var currentBlock = await _rpcService.GetBlockCount();
            return Ok(currentBlock);
        }

        [AllowAnonymous]
        [HttpGet("getipaddresses")]
        public ActionResult<List<string>> GetIpAddresses()
        {
            return Dns.GetHostAddresses(Dns.GetHostName()).Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Select(ip => ip.ToString()).ToList();
        }

        [AllowAnonymous]
        [HttpGet("getmininginfo")]
        public async Task<ActionResult<GetMiningInfoResponse>> GetMiningInfo()
        {
            var hashRate = await _rpcService.GetMiningInfo();
            return Ok(hashRate);
        }

        [AllowAnonymous]
        [HttpGet("getdifficulty")]
        public async Task<ActionResult<GetDifficultyResponse>> GetDifficulty()
        {
            var difficulty = await _rpcService.GetDifficulty();
            return Ok(difficulty);
        }
    }
}