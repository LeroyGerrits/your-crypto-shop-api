using YourCryptoShop.API.Controllers.Attributes;
using YourCryptoShop.API.Controllers.Requests;
using YourCryptoShop.API.Services;
using YourCryptoShop.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using YourCryptoShop.Domain;
using System.Text;
using Microsoft.Extensions.Options;
using YourCryptoShop.Domain.Parameters;
using YourCryptoShop.Domain.Models.ViewModels;
using YourCryptoShop.Domain.Interfaces.Repositories;

namespace YourCryptoShop.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MerchantController(IOptions<AppSettings> appSettings, IHttpContextAccessor httpContextAccessor, IJwtUtils jwtUtils, IAuthenticationService authenticationService, IMailService mailService, IMerchantRepository merchantRepository, IMerchantPasswordResetLinkRepository merchantPasswordResetLinkRepository) : ControllerBase
    {
        private readonly AppSettings _appSettings = appSettings.Value;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IJwtUtils _jwtUtils = jwtUtils;
        private readonly IAuthenticationService _authenticationService = authenticationService;
        private readonly IMailService _mailService = mailService;
        private readonly IMerchantRepository _merchantRepository = merchantRepository;
        private readonly IMerchantPasswordResetLinkRepository _merchantPasswordResetLinkRepository = merchantPasswordResetLinkRepository;

        [AllowAnonymous]
        [HttpGet("public")]
        public async Task<ActionResult<IEnumerable<PublicMerchant>>> GetPublic(string? firstName, string? lastName)
        {
            var merchants = await _merchantRepository.GetPublic(new GetMerchantsParameters()
            {
                FirstName = firstName,
                LastName = lastName
            });
            return Ok(merchants.ToList());
        }

        [AllowAnonymous]
        [HttpGet("public/{id}")]
        public async Task<ActionResult<PublicMerchant>> GetPublic(Guid id)
        {
            var merchant = await _merchantRepository.GetByIdPublic(id);
            if (merchant == null)
                return NotFound();

            return Ok(merchant);
        }

        [MerchantAuthenticationRequired]
        [HttpGet("{id}")]
        public async Task<ActionResult<Merchant>> Get(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var merchant = await _merchantRepository.GetById(authenticatedMerchantId.Value, id);
            if (merchant == null)
                return NotFound();

            return Ok(merchant);
        }

        [AllowAnonymous]
        [HttpGet("{id}/{password}")]
        public async Task<ActionResult<Merchant>> Get(Guid id, string password)
        {
            var merchant = await _merchantRepository.GetByIdAndPassword(id, password);
            if (merchant == null)
                return NotFound();

            return Ok(merchant);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Merchant value)
        {
            value.PasswordSalt = Utilities.GenerateSalt();
            value.Password = Utilities.GenerateRandomString(50);
            var result = await _merchantRepository.Create(value, Guid.Empty);

            if (result.Success)
            {
                string accountActivationUrl = $"{_appSettings.UrlYourCryptoShopWebsite}/account-activate/{result.Identifier}/{value.Password}";

                StringBuilder sbMail = new();
                sbMail.Append($"<p>Hi {value.Username},</p>");
                sbMail.Append($"<p>Your account was registered. Before you can use your account, you will need to activate it. Click on the following link to activate:</p>");
                sbMail.Append($"<p><a href=\"{accountActivationUrl}\">{accountActivationUrl}</a></p>");
                sbMail.Append($"<p>Your Crypto Shop</p>");
                _mailService.SendMail(value.EmailAddress, "Activate your Crypto Shop account", sbMail.ToString());
            }

            return Ok(result);
        }

        [MerchantAuthenticationRequired]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] Merchant value)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var merchant = await _merchantRepository.GetById(authenticatedMerchantId.Value, id);
            if (merchant == null)
                return NotFound();

            var result = await _merchantRepository.Update(value, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPut("activate-account")]
        public async Task<ActionResult> ActivateAccount([FromBody] ActivateAccountRequest request)
        {
            var merchant = await _merchantRepository.GetByIdAndPassword(request.Id, request.CurrentPassword);
            if (merchant == null)
                return NotFound();

            if (merchant.Activated.HasValue)
                return BadRequest("Merchant is already activated.");

            var hashedNewPassword = Utilities.HashStringSha256(merchant.PasswordSalt + request.NewPassword);
            var result = await _merchantRepository.UpdatePasswordAndActivate(merchant, hashedNewPassword, merchant.Id!.Value);
            return Ok(result);
        }

        [MerchantAuthenticationRequired]
        [HttpPut("change-password")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            // First retrieve the merchant by ID so we can get the salt
            var merchantById = await _merchantRepository.GetById(authenticatedMerchantId.Value, authenticatedMerchantId.Value);
            if (merchantById == null)
                return NotFound();

            // Hash the current password using the salt
            var hashedCurrentPassword = Utilities.HashStringSha256(merchantById.PasswordSalt + request.CurrentPassword);

            // Retrieve merchant by ID and hashed password
            var merchant = await _merchantRepository.GetByIdAndPassword(authenticatedMerchantId.Value, hashedCurrentPassword);
            if (merchant == null)
                return BadRequest(new { message = "Your current password is incorrect." });

            // Create a new salt and hash the new password with it
            var newPasswordSalt = Utilities.GenerateSalt();
            var hashedNewPassword = Utilities.HashStringSha256(newPasswordSalt + request.NewPassword);

            var result = await _merchantRepository.UpdatePasswordAndSalt(merchant, hashedNewPassword, newPasswordSalt, merchant.Id!.Value);
            return Ok(result);
        }

        [MerchantAuthenticationRequired]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Merchant>> Delete(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var merchant = await _merchantRepository.GetById(authenticatedMerchantId.Value, id);
            if (merchant == null)
                return NotFound();

            var result = await _merchantRepository.Delete(id, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult> Authenticate([FromBody] AuthenticateRequest request)
        {
            // First retrieve the account by e-mail address so we can get the salt
            var merchantByEmailAddress = await _merchantRepository.GetByEmailAddress(request.EmailAddress);
            if (merchantByEmailAddress == null)
                return BadRequest(new { message = "E-mail address or password is incorrect" });

            // Hash the password using the salt
            request.Password = Utilities.HashStringSha256(merchantByEmailAddress.PasswordSalt + request.Password);

            var response = _authenticationService.AuthenticateMerchant(request);
            if (response == null)
                return BadRequest(new { message = "E-mail address or password is incorrect" });

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("public/forgot-password")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(_appSettings.UrlYourCryptoShopWebsite))
                throw new Exception("Your Crypto Shop Website URL not configured.");

            var merchant = await _merchantRepository.GetByEmailAddress(request.EmailAddress);
            if (merchant == null)
                return Ok(); // Return OK even when using a wrong e-mail address because we want to display a generic message rather than hinting the e-mail address exists

            MerchantPasswordResetLink passwordResetLink = new()
            {
                Merchant = merchant,
                Date = DateTime.UtcNow,
                IpAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                Key = Utilities.GenerateRandomString(50)
            };

            var result = await _merchantPasswordResetLinkRepository.Create(passwordResetLink, Guid.Empty);
            if (result.Success)
            {
                string passwordResetUrl = $"{_appSettings.UrlYourCryptoShopWebsite}/reset-password/{result.Identifier}/{passwordResetLink.Key}";

                StringBuilder sbMail = new();
                sbMail.Append($"<p>Hi {merchant.Username},</p>");
                sbMail.Append($"<p>A password reset for your account was requested. If this was you, click on the following link to proceed setting a new password:</p>");
                sbMail.Append($"<p><a href=\"{passwordResetUrl}\">{passwordResetUrl}</a></p>");
                sbMail.Append($"<p>If this wasn't you, ignore this link.</p>");
                sbMail.Append($"<p>Your Crypto Shop</p>");
                _mailService.SendMail(merchant.EmailAddress, "Reset your Crypto Shop password", sbMail.ToString());
            }

            return Ok();
        }
    }
}