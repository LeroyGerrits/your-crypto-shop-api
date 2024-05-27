using YourCryptoShop.API.Controllers.Requests;
using YourCryptoShop.Domain;
using YourCryptoShop.Domain.Interfaces.Repositories;
using YourCryptoShop.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YourCryptoShop.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MerchantPasswordResetLinkController(IMerchantRepository merchantRepository, IMerchantPasswordResetLinkRepository merchantPasswordResetLinkRepository) : ControllerBase
    {
        private readonly IMerchantRepository _merchantRepository = merchantRepository;
        private readonly IMerchantPasswordResetLinkRepository _merchantPasswordResetLinkRepository = merchantPasswordResetLinkRepository;

        [AllowAnonymous]
        [HttpGet("public")]
        public async Task<ActionResult<DeliveryMethod>> Get(Guid id, string key)
        {
            var merchantPasswordResetLink = await _merchantPasswordResetLinkRepository.GetByIdAndKey(id, key);
            if (merchantPasswordResetLink == null)
                return NotFound();

            return Ok(merchantPasswordResetLink);
        }

        [AllowAnonymous]
        [HttpPut("public/reset-password")]
        public async Task<ActionResult> Put([FromBody] ResetPasswordRequest request)
        {
            var merchantPasswordResetLink = await _merchantPasswordResetLinkRepository.GetByIdAndKey(request.Id, request.Key);
            if (merchantPasswordResetLink == null)
                return BadRequest(new { message = "Password reset link already used or expired." });

            // Create a new salt and hash the new password with it
            var newPasswordSalt = Utilities.GenerateSalt();
            var hashedNewPassword = Utilities.HashStringSha256(newPasswordSalt + request.Password);

            var result = await _merchantRepository.UpdatePasswordAndSalt(merchantPasswordResetLink.Merchant, hashedNewPassword, newPasswordSalt, merchantPasswordResetLink.Merchant.Id!.Value);

            if (result.Success)
                await _merchantPasswordResetLinkRepository.UpdateUsed(merchantPasswordResetLink, merchantPasswordResetLink.Merchant.Id!.Value);

            return Ok(result);
        }
    }
}