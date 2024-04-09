using DGBCommerce.Domain;
using DGBCommerce.Domain.Enums;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Models.ViewModels;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Data.Repositories
{
    public class OrderRepository(IDataAccessLayer dataAccessLayer) : IOrderRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<Order>> Get(GetOrdersParameters parameters)
            => await GetRaw(parameters);

        public async Task<IEnumerable<Order>> GetByStatus(OrderStatus status)
            => await GetRaw(new GetOrdersParameters() { Status = status });

        public async Task<Order?> GetById(Guid merchantId, Guid id)
        {
            var shops = await GetRaw(new GetOrdersParameters() { MerchantId = merchantId, Id = id });
            return shops.ToList().SingleOrDefault();
        }

        public async Task<IEnumerable<PublicOrder>> GetPublic(GetOrdersParameters parameters)
            => await GetRawPublic(parameters);

        public async Task<PublicOrder?> GetByIdPublic(Guid shopId, Guid id)
        {
            var orders = await GetRawPublic(new GetOrdersParameters() { ShopId = shopId, Id = id });
            return orders.ToList().SingleOrDefault();
        }

        public Task<MutationResult> Create(Order item, Guid mutationId)
            => _dataAccessLayer.CreateOrder(item, mutationId);

        public Task<MutationResult> Update(Order item, Guid mutationId)
            => _dataAccessLayer.UpdateOrder(item, mutationId);

        public Task<MutationResult> UpdateStatus(Order item, OrderStatus status, Guid mutationId)
            => _dataAccessLayer.UpdateOrderStatus(item.Id!.Value, status, mutationId);

        public Task<MutationResult> UpdateTransaction(Order item, Guid transactionId, Guid mutationId)
            => _dataAccessLayer.UpdateOrderTransaction(item.Id!.Value, transactionId, mutationId);

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => _dataAccessLayer.DeleteOrder(id, mutationId);

        private async Task<IEnumerable<Order>> GetRaw(GetOrdersParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetOrders(parameters);
            List<Order> orders = [];

            foreach (DataRow row in table.Rows)
            {
                var order = new Order()
                {
                    Id = new Guid(row["ord_id"].ToString()!),
                    ShopId = new Guid(row["ord_shop"].ToString()!),
                    Customer = new()
                    {
                        Id = new Guid(row["ord_customer"].ToString()!),
                        ShopId = new Guid(row["ord_shop"].ToString()!),
                        EmailAddress = Utilities.DbNullableString(row["ord_customer_email_address"]),
                        Username = Utilities.DbNullableString(row["ord_customer_username"]),
                        Gender = (Gender)Convert.ToInt32(row["ord_customer_gender"]),
                        FirstName = Utilities.DbNullableString(row["ord_customer_first_name"]),
                        LastName = Utilities.DbNullableString(row["ord_customer_last_name"]),
                    },
                    Date = Convert.ToDateTime(row["ord_date"]),
                    Status = (OrderStatus)Convert.ToInt32(row["ord_status"]),
                    BillingAddress = new()
                    {
                        AddressLine1 = Utilities.DbNullableString(row["ord_address_billing_address_line_1"]),
                        AddressLine2 = Utilities.DbNullableString(row["ord_address_billing_address_line_2"]),
                        PostalCode = Utilities.DbNullableString(row["ord_address_billing_postal_code"]),
                        City = Utilities.DbNullableString(row["ord_address_billing_city"]),
                        Province = Utilities.DbNullableString(row["ord_address_billing_country"]),
                        Country = new()
                        {
                            Id = new Guid(row["ord_address_billing_country"].ToString()!),
                            Code = Utilities.DbNullableString(row["ord_address_billing_country_code"]),
                            Name = Utilities.DbNullableString(row["ord_address_billing_country_name"])
                        }
                    },
                    ShippingAddress = new()
                    {
                        AddressLine1 = Utilities.DbNullableString(row["ord_address_shipping_address_line_1"]),
                        AddressLine2 = Utilities.DbNullableString(row["ord_address_shipping_address_line_2"]),
                        PostalCode = Utilities.DbNullableString(row["ord_address_shipping_postal_code"]),
                        City = Utilities.DbNullableString(row["ord_address_shipping_city"]),
                        Province = Utilities.DbNullableString(row["ord_address_shipping_country"]),
                        Country = new()
                        {
                            Id = new Guid(row["ord_address_shipping_country"].ToString()!),
                            Code = Utilities.DbNullableString(row["ord_address_shipping_country_code"]),
                            Name = Utilities.DbNullableString(row["ord_address_shipping_country_name"])
                        }
                    },
                    DeliveryMethodId = new Guid(row["ord_delivery_method"].ToString()!),
                    Comments = row["ord_comments"].ToString(),
                    SenderWalletAddress = row["ord_sender_wallet_address"].ToString()
                };

                if (row["ord_transaction"] != DBNull.Value)
                {
                    order.Transaction = new Transaction()
                    {
                        Id = new Guid(row["ord_transaction"].ToString()!),
                        ShopId = new Guid(row["ord_shop"].ToString()!),
                        Recipient = Utilities.DbNullableString(row["ord_transaction_recipient"]),
                        AmountDue = Convert.ToDecimal(row["ord_transaction_amount_due"]),
                        AmountPaid = Convert.ToDecimal(row["ord_transaction_amount_paid"]),
                        PaidInFull = Utilities.DBNullableDateTime(row["ord_transaction_paid_in_full"]),
                        Tx = Utilities.DbNullableString(row["ord_transaction_tx"])
                    };
                }

                orders.Add(order);
            }

            return orders;
        }

        private async Task<IEnumerable<PublicOrder>> GetRawPublic(GetOrdersParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetOrders(parameters);
            List<PublicOrder> orders = [];

            foreach (DataRow row in table.Rows)
            {
                orders.Add(new PublicOrder()
                {
                    Id = new Guid(row["ord_id"].ToString()!),
                    ShopId = new Guid(row["ord_shop"].ToString()!),
                    CustomerId = new Guid(row["ord_customer"].ToString()!),
                    Date = Convert.ToDateTime(row["ord_date"]),
                    Status = (OrderStatus)Convert.ToInt32(row["ord_status"]),
                    BillingAddress = new()
                    {
                        AddressLine1 = Utilities.DbNullableString(row["ord_address_billing_address_line_1"]),
                        AddressLine2 = Utilities.DbNullableString(row["ord_address_billing_address_line_2"]),
                        PostalCode = Utilities.DbNullableString(row["ord_address_billing_postal_code"]),
                        City = Utilities.DbNullableString(row["ord_address_billing_city"]),
                        Province = Utilities.DbNullableString(row["ord_address_billing_country"]),
                        Country = new()
                        {
                            Id = new Guid(row["ord_address_billing_country"].ToString()!),
                            Code = Utilities.DbNullableString(row["ord_address_billing_country_code"]),
                            Name = Utilities.DbNullableString(row["ord_address_billing_country_name"])
                        }
                    },
                    ShippingAddress = new()
                    {
                        AddressLine1 = Utilities.DbNullableString(row["ord_address_shipping_address_line_1"]),
                        AddressLine2 = Utilities.DbNullableString(row["ord_address_shipping_address_line_2"]),
                        PostalCode = Utilities.DbNullableString(row["ord_address_shipping_postal_code"]),
                        City = Utilities.DbNullableString(row["ord_address_shipping_city"]),
                        Province = Utilities.DbNullableString(row["ord_address_shipping_country"]),
                        Country = new()
                        {
                            Id = new Guid(row["ord_address_shipping_country"].ToString()!),
                            Code = Utilities.DbNullableString(row["ord_address_shipping_country_code"]),
                            Name = Utilities.DbNullableString(row["ord_address_shipping_country_name"])
                        }
                    },
                    DeliveryMethodId = new Guid(row["ord_delivery_method"].ToString()!),
                    Comments = row["ord_comments"].ToString(),
                    SenderWalletAddress = row["ord_sender_wallet_address"].ToString(),
                    TransactionId = Utilities.DbNullableGuid(row["ord_transaction"]),
                    TransactionRecipient = Utilities.DbNullableString(row["ord_transaction_recipient"]),
                    TransactionAmountDue = Convert.ToDecimal(row["ord_transaction_amount_due"]),
                    TransactionAmountPaid = Convert.ToDecimal(row["ord_transaction_amount_paid"]),
                    TransactionPaidInFull = Utilities.DBNullableDateTime(row["ord_transaction_paid_in_full"]),
                    TransactionTx = Utilities.DbNullableString(row["ord_transaction_tx"])
                });
            }

            return orders;
        }
    }
}