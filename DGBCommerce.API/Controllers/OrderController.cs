using DGBCommerce.API.Controllers.Attributes;
using DGBCommerce.API.Controllers.Requests;
using DGBCommerce.API.Services;
using DGBCommerce.Domain;
using DGBCommerce.Domain.Enums;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Interfaces.Services;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Models.ViewModels;
using DGBCommerce.Domain.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text;
using System.Web;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController(
        IOptions<AppSettings> appSettings,
        IAddressService addressService,
        ICustomerRepository customerRepository,
        IDeliveryMethodRepository deliveryMethodRepository,
        IHttpContextAccessor httpContextAccessor,
        IJwtUtils jwtUtils,
        IMailService mailService,
        IMerchantRepository merchantRepository,
        IOrderRepository orderRepository,
        IOrderItemRepository orderItemRepository,
        IRpcService rpcService,
        IShopRepository shopRepository,
        IShoppingCartRepository shoppingCartRepository,
        IShoppingCartItemRepository shoppingCartItemRepository,
        ITransactionRepository transactionRepository) : ControllerBase
    {
        private readonly AppSettings _appSettings = appSettings.Value;

        [MerchantAuthenticationRequired]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> Get(Guid? shopId, Guid? customerId, string? customer, DateTime? dateFrom, DateTime? dateUntil, OrderStatus? status)
        {
            var authenticatedMerchantId = jwtUtils.GetMerchantId(httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var orders = await orderRepository.Get(new GetOrdersParameters()
            {
                MerchantId = authenticatedMerchantId.Value,
                ShopId = shopId,
                CustomerId = customerId,
                Customer = customer,
                DateFrom = dateFrom,
                DateUntil = dateUntil,
                Status = status
            });
            return Ok(orders.ToList());
        }

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

            var merchant = await merchantRepository.GetById(shop.MerchantId, shop.MerchantId);
            if (merchant == null)
                return NotFound(new { message = "Merchant not found." });

            var deliveryMethod = await deliveryMethodRepository.GetByIdPublic(value.DeliveryMethodId);
            if (deliveryMethod == null)
                return BadRequest(new { message = "Delivery method not found." });

            var address = await addressService.GetAddress(value.AddressLine1, value.AddressLine2, value.PostalCode, value.City, value.Province, value.CountryId);
            if (address == null)
                return BadRequest(new { message = "Could not retrieve address record." });

            Customer? customer = null;

            if (value.CustomerId != null)
                customer = await customerRepository.GetById(value.CustomerId.Value!);

            customer ??= await customerRepository.GetByEmailAddress(shop.Id, value.EmailAddress);

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
                Shop = new Shop()
                {
                    Id = shop.Id,
                    MerchantId = shop.MerchantId,
                    Name = shop.Name
                },
                Customer = customer,
                Date = DateTime.UtcNow,
                Status = (shop.OrderMethod == ShopOrderMethod.Automated ? OrderStatus.AwaitingPayment : OrderStatus.New),
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
                var shoppingCartItems = await shoppingCartItemRepository.GetByShoppingCartId(shoppingCart.Id!.Value);

                // Shopping cart items
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

                // Calculate cumulative amount
                var cumulativeAmount = orderItemsToCreate.Sum(i => i.Amount * i.Price);

                // Mail template
                string salutation = "Mr./Ms.";

                if (customer.Gender == Gender.Male) salutation = "Mr.";
                if (customer.Gender == Gender.Female) salutation = "Ms.";

                StringBuilder sbMail = new();
                sbMail.Append("<head>");
                sbMail.Append("<style>");
                sbMail.Append("table { border-collapse:collapse; }");
                sbMail.Append("td, th { border:1px solid #aaa; padding:5px; }");
                sbMail.Append("</style>");
                sbMail.Append("</head>");
                sbMail.Append("<body>");
                sbMail.Append($"<p>Hi {salutation} {customer.FirstName} {customer.LastName},</p>");
                sbMail.Append($"<p>Your order on {shop.Name} was succesfully created.</p>");
                sbMail.Append($"<table>");
                sbMail.Append($"<tr>");
                sbMail.Append($"<th style=\"text-align:left;\">Product</th>");
                sbMail.Append($"<th style=\"text-align:right;\">Amount</th>");
                sbMail.Append($"<th style=\"text-align:right;\">Price</th>");
                sbMail.Append($"<th style=\"text-align:right;\">Total</th>");
                sbMail.Append($"</tr>");

                foreach (var shoppingCartItem in shoppingCartItems)
                {
                    sbMail.Append($"<tr>");
                    sbMail.Append($"<td>{HttpUtility.HtmlEncode(shoppingCartItem.ProductName)}</td>");
                    sbMail.Append($"<td style=\"text-align:right;\">{shoppingCartItem.Amount}</td>");
                    sbMail.Append($"<td style=\"text-align:right;\">Ɗ&nbsp;{shoppingCartItem.ProductPrice!.Value.ToString("#.########")}</td>");
                    sbMail.Append($"<td style=\"text-align:right;\">Ɗ&nbsp;{shoppingCartItem.Total.ToString("#.########")}</td>");
                    sbMail.Append($"</tr>");
                }

                sbMail.Append($"<tr>");
                sbMail.Append($"<td colspan=\"3\">{HttpUtility.HtmlEncode(deliveryMethod.Name)}</td>");

                if (deliveryMethod.Costs.HasValue)
                    sbMail.Append($"<td style=\"text-align:right;\">Ɗ&nbsp;{deliveryMethod.Costs.Value.ToString("#.########")}</td>");
                else
                    sbMail.Append($"<td></td>");
                sbMail.Append($"</tr>");

                sbMail.Append($"<tr>");
                sbMail.Append($"<td colspan=\"4\" style=\"text-align:right;\"><strong>Ɗ&nbsp;{cumulativeAmount.ToString("#.########")}</strong></td>");
                sbMail.Append($"</tr>");
                sbMail.Append($"</table>");

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
                        Recipient = newDigiByteAddress,
                        AmountDue = cumulativeAmount,
                        AmountPaid = 0
                    };

                    var resultTransaction = await transactionRepository.Create(transactionToCreate, Guid.Empty);
                    if (resultTransaction.Success)
                    {
                        var resultOrderTransaction = await orderRepository.UpdateTransaction(orderToCreate, newTransactionId, Guid.Empty);
                        string shopSubDomain = !string.IsNullOrEmpty(shop.SubDomain) ? shop.SubDomain : shop.Id.ToString();
                        string orderStatusUrl = $"https://{shopSubDomain}.dgbcommerce.com/order-status/{orderToCreate.Id}";
                        sbMail.Append($"<p>Should you have been unable to complete the payment after placing your order, you can still do so by navigating to the following link:</p>");
                        sbMail.Append($"<p><a href=\"{orderStatusUrl}\">{orderStatusUrl}</a></p>");
                    }
                }

                // If shop order method requires manual action, just send an e-mail with a confirmation.
                // Once the merchant updates the order status, the customer will receive a separate e-mail with a payment link.
                if (shop.OrderMethod == ShopOrderMethod.ManualActionRequired)
                {
                    sbMail.Append($"<p>You will receive additional information and payment instructions as soon as the merchant processes your order.</p>");
                }

                sbMail.Append($"<p>DGB Commerce</p>");
                sbMail.Append("</body>");
                mailService.SendMail(customer.EmailAddress, $"Your order on {shop.Name}", sbMail.ToString());
                mailService.SendMail(merchant.EmailAddress, $"New order placed on {shop.Name} (COPY)", sbMail.ToString());
            }

            return Ok(resultOrder);
        }



        [AllowAnonymous]
        [HttpGet("public/{shopId}/{id}")]
        public async Task<ActionResult<IEnumerable<PublicOrder>>> GetPublicById(Guid shopId, Guid id)
        {
            var order = await orderRepository.GetByIdPublic(shopId, id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }
    }
}