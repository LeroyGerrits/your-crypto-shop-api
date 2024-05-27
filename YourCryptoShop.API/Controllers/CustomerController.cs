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
using YourCryptoShop.Domain.Interfaces.Repositories;
using YourCryptoShop.Domain.Interfaces.Services;

namespace YourCryptoShop.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController(IOptions<AppSettings> appSettings, IHttpContextAccessor httpContextAccessor, IJwtUtils jwtUtils, IAddressService addressService, IAuthenticationService authenticationService, IMailService mailService, ICustomerRepository customerRepository) : ControllerBase
    {
        private readonly AppSettings _appSettings = appSettings.Value;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IJwtUtils _jwtUtils = jwtUtils;
        private readonly IAddressService _addressService = addressService;
        private readonly IAuthenticationService _authenticationService = authenticationService;
        private readonly IMailService _mailService = mailService;
        private readonly ICustomerRepository _customerRepository = customerRepository;

        [MerchantAuthenticationRequired]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> Get(Guid? shopId, string? username, string? emailAddress, string? firstName, string? lastName)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var customers = await _customerRepository.Get(new GetCustomersParameters()
            {
                MerchantId = authenticatedMerchantId.Value,
                ShopId = shopId,
                Username = username,
                EmailAddress = emailAddress,
                FirstName = firstName,
                LastName = lastName
            });
            return Ok(customers.ToList());
        }

        [MerchantAuthenticationRequired]
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> Get(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var customer = await _customerRepository.GetById(authenticatedMerchantId.Value, id);
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [AllowAnonymous]
        [HttpGet("{id}/{password}")]
        public async Task<ActionResult<Customer>> Get(Guid id, string password)
        {
            var customer = await _customerRepository.GetByIdAndPassword(id, password);
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [MerchantAuthenticationRequired]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MutateCustomerRequest value)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var address = await _addressService.GetAddress(value.AddressLine1, value.AddressLine2, value.PostalCode, value.City, value.Province, value.Country.Id!.Value);
            if (address == null)
                return BadRequest(new { message = "Could not retrieve address record." });

            value.Customer.Address = address;
            value.Customer.PasswordSalt = Utilities.GenerateSalt();
            value.Customer.Password = Utilities.GenerateRandomString(50);

            var result = await _customerRepository.Create(value.Customer, authenticatedMerchantId.Value);
            if (result.Success && false) // TO-DO: Send e-mail with activation link pointing to merchant shop
            {
                string accountActivationUrl = $"{_appSettings.UrlDgbCommerceWebsite}/account-activate/{result.Identifier}/{value.Customer.Password}";

                StringBuilder sbMail = new();
                sbMail.Append($"<p>Hi {value.Customer.Username},</p>");
                sbMail.Append($"<p>An account for you was created. Before you can use your account, you will need to activate it. Click on the following link to activate:</p>");
                sbMail.Append($"<p><a href=\"{accountActivationUrl}\">{accountActivationUrl}</a></p>");
                sbMail.Append($"<p>DGB Commerce</p>");
                _mailService.SendMail(value.Customer.EmailAddress, "Activate your DGB Commerce account", sbMail.ToString());
            }

            return Ok(result);
        }

        [MerchantAuthenticationRequired]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] MutateCustomerRequest value)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var customer = await _customerRepository.GetById(authenticatedMerchantId.Value, id);
            if (customer == null)
                return NotFound();

            var address = await _addressService.GetAddress(value.AddressLine1, value.AddressLine2, value.PostalCode, value.City, value.Province, value.Country.Id!.Value);
            if (address == null)
                return BadRequest(new { message = "Could not retrieve address record." });

            value.Customer.Address = address;

            var result = await _customerRepository.Update(value.Customer, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPut("activate-account")]
        public async Task<ActionResult> ActivateAccount([FromBody] ActivateAccountRequest request)
        {
            var customer = await _customerRepository.GetByIdAndPassword(request.Id, request.CurrentPassword);
            if (customer == null)
                return NotFound();

            if (customer.Activated.HasValue)
                return BadRequest("Customer is already activated.");

            var hashedNewPassword = Utilities.HashStringSha256(customer.PasswordSalt + request.NewPassword);
            var result = await _customerRepository.UpdatePasswordAndActivate(customer, hashedNewPassword, customer.Id!.Value);
            return Ok(result);
        }

        [MerchantAuthenticationRequired]
        [HttpPut("change-password")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            // First retrieve the customer by ID so we can get the salt
            var customerById = await _customerRepository.GetById(authenticatedMerchantId.Value, authenticatedMerchantId.Value);
            if (customerById == null)
                return NotFound();

            // Hash the current password using the salt
            var hashedCurrentPassword = Utilities.HashStringSha256(customerById.PasswordSalt + request.CurrentPassword);

            // Retrieve customer by ID and hashed password
            var customer = await _customerRepository.GetByIdAndPassword(authenticatedMerchantId.Value, hashedCurrentPassword);
            if (customer == null)
                return BadRequest(new { message = "Your current password is incorrect." });

            // Create a new salt and hash the new password with it
            var newPasswordSalt = Utilities.GenerateSalt();
            var hashedNewPassword = Utilities.HashStringSha256(newPasswordSalt + request.NewPassword);

            var result = await _customerRepository.UpdatePasswordAndSalt(customer, hashedNewPassword, newPasswordSalt, customer.Id!.Value);
            return Ok(result);
        }

        [MerchantAuthenticationRequired]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> Delete(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var customer = await _customerRepository.GetById(authenticatedMerchantId.Value, id);
            if (customer == null)
                return NotFound();

            var result = await _customerRepository.Delete(id, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult> Authenticate([FromBody] AuthenticateCustomerRequest request)
        {
            // First retrieve the account by e-mail address so we can get the salt
            var customerByEmailAddress = await _customerRepository.GetByEmailAddress(request.ShopId, request.EmailAddress);
            if (customerByEmailAddress == null)
                return BadRequest(new { message = "E-mail address or password is incorrect" });

            // Hash the password using the salt
            request.Password = Utilities.HashStringSha256(customerByEmailAddress.PasswordSalt + request.Password);

            var response = _authenticationService.AuthenticateCustomer(request);
            if (response == null)
                return BadRequest(new { message = "E-mail address or password is incorrect" });

            return Ok(response);
        }
    }
}