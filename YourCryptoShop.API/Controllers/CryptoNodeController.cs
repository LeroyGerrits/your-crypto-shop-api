using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace YourCryptoShop.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CryptoNodeController(
        IUtils utils
        ) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("getblockcount/{coin}")]
        public async Task<ActionResult<uint>> GetBlockCount(string coin)
        {
            var currentBlock = await utils.GetRpcService(coin).GetBlockCount();
            return Ok(currentBlock);
        }

        [AllowAnonymous]
        [HttpGet("getipaddresses")]
        public ActionResult<List<string>> GetIpAddresses()
        {
            return Dns.GetHostAddresses(Dns.GetHostName()).Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Select(ip => ip.ToString()).ToList();
        }

        [AllowAnonymous]
        [HttpGet("getnewaddress/{coin}")]
        public async Task<ActionResult<string>> GetNewAddress(string coin)
        {
            var newAddress = await utils.GetRpcService(coin).GetNewAddress();
            return Ok(newAddress);
        }

        [AllowAnonymous]
        [HttpGet("getnewaddress/{coin}/{label}")]
        public async Task<ActionResult<string>> GetNewAddress(string coin, string? label)
        {
            var newAddress = await utils.GetRpcService(coin).GetNewAddress(label);
            return Ok(newAddress);
        }

        [AllowAnonymous]
        [HttpGet("getnewaddress/{coin}/{label}/{addressType}")]
        public async Task<ActionResult<string>> GetNewAddress(string coin, string? label, string? addressType)
        {
            var newAddress = await utils.GetRpcService(coin).GetNewAddress(label, addressType);
            return Ok(newAddress);
        }
    }
}