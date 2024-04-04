using DGBCommerce.API.Controllers.Requests;
using DGBCommerce.API.Services;
using DGBCommerce.Domain;
using DGBCommerce.Domain.Enums;
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
        IDeliveryMethodRepository deliveryMethodRepository,
        IRpcService rpcService,
        IOrderRepository orderRepository,
        IOrderItemRepository orderItemRepository,
        IShopRepository shopRepository,
        IShoppingCartRepository shoppingCartRepository,
        IShoppingCartItemRepository shoppingCartItemRepository,
        ITransactionRepository transactionRepository) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("public")]
        public async Task<ActionResult> Post([FromBody] CreateOrderRequest value)
        {
            var shoppingCart = await shoppingCartRepository.GetBySession(value.SessionId);
            if (shoppingCart == null)
                return NotFound(new { message = "Shopping cart not found." });

            var shop = await shopRepository.GetByIdPublic(value.ShopId);
            if (shop == null)
                return NotFound(new { message = "Shop not found." });

            if (!shop.HasWallet)
                return NotFound(new { message = "Shop has no wallet configured." });

            var deliveryMethod = await deliveryMethodRepository.GetByIdPublic(value.DeliveryMethodId);
            if (deliveryMethod == null)
                return BadRequest(new { message = "Delivery method not found." });

            var address = await addressService.GetAddress(value.AddressLine1, value.AddressLine2, value.PostalCode, value.City, value.Province, value.CountryId);
            if (address == null)
                return BadRequest(new { message = "Could not retrieve address record." });

            Customer? customer = null;

            if (value.CustomerId != null)
                customer = await customerRepository.GetById(value.CustomerId.Value!);

            if (customer == null)
            {
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

                var resultCustomer = await customerRepository.Create(customer, Guid.Empty);
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
                Status = OrderStatus.New,
                BillingAddress = address,
                ShippingAddress = address,
                DeliveryMethodId = value.DeliveryMethodId,
                Comments = value.Comments
            };

            var resultOrder = await orderRepository.Create(orderToCreate, Guid.Empty);
            if (resultOrder.Success)
            {
                // Creating the order was successfull, now create the order items
                orderToCreate.Id = resultOrder.Identifier;
                List<OrderItem> orderItemsToCreate = [];

                // Shopping cart items
                var shoppingCartItems = await shoppingCartItemRepository.GetByShoppingCartId(shoppingCart.Id!.Value);
                foreach (var shoppingCartItem in shoppingCartItems)
                {
                    orderItemsToCreate.Add(new()
                    {
                        OrderId = orderToCreate.Id.Value,
                        Type = OrderItemType.ShoppingCartItem,
                        Amount = shoppingCartItem.Amount,
                        Price = shoppingCartItem.ProductPrice!.Value,
                        Description = shoppingCartItem.ProductName!
                    });
                }

                // Delivery method
                orderItemsToCreate.Add(new()
                {
                    OrderId = orderToCreate.Id.Value,
                    Type = OrderItemType.DeliveryMethod,
                    Amount = 1,
                    Price = deliveryMethod.Costs ?? 0,
                    Description = deliveryMethod.Name
                });

                foreach (var orderItemToCreate in orderItemsToCreate)
                    await orderItemRepository.Create(orderItemToCreate, Guid.Empty);

                // If shop order method is 'Automated', create a transaction and e-mail customer with payment link
                if (shop.OrderMethod == ShopOrderMethod.Automated)
                {
                    Guid newTransactionId = Guid.NewGuid();
                    var newDigiByteAddress = await rpcService.GetNewAddress($"DGB Commerce Transaction {newTransactionId}");
                    var transactionToCreate = new Transaction()
                    {
                        Id = newTransactionId,
                        ShopId = shop.Id,
                        Amount = orderItemsToCreate.Sum(i => i.Amount * i.Price),
                        Recipient = newDigiByteAddress
                    };

                    var resultTransaction = await transactionRepository.Create(transactionToCreate, Guid.Empty);
                    if (resultTransaction.Success)
                    {
                        var resultOrderTransaction = await orderRepository.UpdateTransaction(orderToCreate, newTransactionId, Guid.Empty);
                        
                        // Send e-mail
                    }
                }

                // If shop order method requires manual action, just send an e-mail with a confirmation.
                // Once the merchant updates the order status, the customer will receive a separate e-mail with a payment link.
                if (shop.OrderMethod == ShopOrderMethod.ManualActionRequired)
                {
                    // Send e-mail
                }
            }

            return Ok(resultOrder);
        }
    }
}