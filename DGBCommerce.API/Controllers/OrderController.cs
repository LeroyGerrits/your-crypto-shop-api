using DGBCommerce.API.Controllers.Requests;
using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Interfaces.Services;
using DGBCommerce.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController(
        IAddressService addressService,
        ICustomerRepository customerRepository,
        IOrderRepository orderRepository,
        IShopRepository shopRepository,
        IShoppingCartRepository shoppingCartRepository,
        IShoppingCartItemRepository shoppingCartItemRepository) : ControllerBase
    {
        private readonly IAddressService _addressService = addressService;
        private readonly ICustomerRepository _customerRepository = customerRepository;
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IShopRepository _shopRepository = shopRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository = shoppingCartRepository;
        private readonly IShoppingCartItemRepository _shoppingCartItemRepository = shoppingCartItemRepository;

        [AllowAnonymous]
        [HttpPost("public")]
        public async Task<ActionResult> Post([FromBody] CreateOrderRequest value)
        {
            var shop = await _shopRepository.GetByIdPublic(value.ShopId);
            if (shop == null)
                return NotFound(new { message = "Shop not found." });

            var address = await _addressService.GetAddress(value.AddressLine1, value.AddressLine2, value.PostalCode, value.City, value.Province, value.CountryId);
            if (address == null)
                return BadRequest(new { message = "Could not retrieve address record." });

            var customer = await _customerRepository.GetByEmailAddress(value.ShopId, value.EmailAddress);
            if (customer == null)
            {
                // Create a new salt and hash the new password with it
                var newPasswordSalt = Utilities.GenerateSalt();
                var newPassword = Utilities.GenerateRandomString(50);
                var hashedNewPassword = Utilities.HashStringSha256(newPasswordSalt + newPassword);

                customer = new Customer()
                {
                    ShopId = value.ShopId,
                    PasswordSalt = newPasswordSalt,
                    Password = hashedNewPassword,
                    EmailAddress = value.EmailAddress,
                    Username = value.EmailAddress,
                    Gender = value.Gender,
                    FirstName = value.FirstName,
                    LastName = value.LastName,
                    Address = address
                    
                };

                var resultCustomer = await _customerRepository.Create(customer, Guid.Empty);
                if (resultCustomer.Success)
                    customer.Id = resultCustomer.Identifier;
                else
                    return BadRequest(new { message = $"Could not create customer: {resultCustomer.Message}" });
            }

            var orderToCreate = new Order()
            {
                ShopId = value.ShopId,
                Customer = customer,
                Date = DateTime.UtcNow,
                Status = Domain.Enums.OrderStatus.New,                
                BillingAddress = address,
                ShippingAddress = address,                
                DeliveryMethodId = value.DeliveryMethodId,
                Comments = value.Comments
            };

            var resultOrder = await _orderRepository.Create(orderToCreate, Guid.Empty);
            if (resultOrder.Success)
            {
                // TO-DO: Convert shopping cart items to order items
            }

            return Ok(resultOrder);
        }
    }
}