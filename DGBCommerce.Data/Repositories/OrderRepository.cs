using DGBCommerce.Domain;
using DGBCommerce.Domain.Enums;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
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
                orders.Add(new Order()
                {
                    Id = new Guid(row["ord_id"].ToString()!),
                    ShopId = new Guid(row["ord_shop"].ToString()!),
                    Customer = new()
                    {
                        Id = new Guid(row["ord_customer"].ToString()!),
                        ShopId = new Guid(row["ord_customer_shop"].ToString()!),
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
                    TransactionId = Utilities.DbNullableGuid(row["ord_transaction"])
                });
            }

            return orders;
        }
    }
}