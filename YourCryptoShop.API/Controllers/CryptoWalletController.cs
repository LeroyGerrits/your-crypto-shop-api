using YourCryptoShop.API.Controllers.Attributes;
using YourCryptoShop.Domain.Exceptions;
using YourCryptoShop.Domain.Interfaces.Repositories;
using YourCryptoShop.Domain.Interfaces.Services;
using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;
using Microsoft.AspNetCore.Mvc;

namespace YourCryptoShop.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CryptoWalletController(IHttpContextAccessor httpContextAccessor, IJwtUtils jwtUtils, ICryptoWalletRepository cryptoWalletRepository, IRpcService rpcService) : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IJwtUtils _jwtUtils = jwtUtils;
        private readonly ICryptoWalletRepository _cryptoWalletRepository = cryptoWalletRepository;
        private readonly IRpcService _rpcService = rpcService;

        [MerchantAuthenticationRequired]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CryptoWallet>>> Get(Guid? currencyId, string? name, string? address)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var cryptoWallets = await _cryptoWalletRepository.Get(new GetCryptoWalletsParameters()
            {
                MerchantId = authenticatedMerchantId.Value,
                CurrencyId = currencyId,
                Name = name,
                Address = address
            });
            return Ok(cryptoWallets.ToList());
        }

        [MerchantAuthenticationRequired]
        [HttpGet("{id}")]
        public async Task<ActionResult<CryptoWallet>> Get(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var cryptoWallet = await _cryptoWalletRepository.GetById(authenticatedMerchantId.Value, id);
            if (cryptoWallet == null)
                return NotFound();

            return Ok(cryptoWallet);
        }

        [MerchantAuthenticationRequired]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CryptoWallet value)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            try
            {
                var validateAddressResponse = await _rpcService.ValidateAddress(value.Address);
                if (!validateAddressResponse.IsValid)
                    return BadRequest(new { message = "The address you supplied is not valid." });
            }
            catch (RpcException)
            {
                return BadRequest(new { message = "Node could not be contacted." });
            }

            var result = await _cryptoWalletRepository.Create(value, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [MerchantAuthenticationRequired]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] CryptoWallet value)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            try
            {
                var validateAddressResponse = await _rpcService.ValidateAddress(value.Address);
                if (!validateAddressResponse.IsValid)
                    return BadRequest(new { message = "The Crypto address you supplied is not valid." });
            }
            catch (RpcException)
            {
                return BadRequest(new { message = "Crypto node could not be contacted." });
            }

            var cryptoWallet = await _cryptoWalletRepository.GetById(authenticatedMerchantId.Value, id);
            if (cryptoWallet == null)
                return NotFound();

            var result = await _cryptoWalletRepository.Update(value, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [MerchantAuthenticationRequired]
        [HttpDelete("{id}")]
        public async Task<ActionResult<CryptoWallet>> Delete(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var cryptoWallet = await _cryptoWalletRepository.GetById(authenticatedMerchantId.Value, id);
            if (cryptoWallet == null)
                return NotFound();

            var result = await _cryptoWalletRepository.Delete(id, authenticatedMerchantId.Value);
            return Ok(result);
        }
    }
}