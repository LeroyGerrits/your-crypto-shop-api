﻿using DGBCommerce.Domain;
using DGBCommerce.Domain.Enums;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Data.Repositories
{
    public class OrderItemRepository(IDataAccessLayer dataAccessLayer) : IOrderItemRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<OrderItem>> Get(GetOrderItemsParameters parameters)
            => await GetRaw(parameters);

        public async Task<IEnumerable<OrderItem>> GetByOrderId(Guid orderId)
            => await GetRaw(new GetOrderItemsParameters() { OrderId = orderId });

        public Task<OrderItem?> GetById(Guid merchantId, Guid id)
            => throw new InvalidOperationException($"{nameof(ShoppingCart)} objects can not be retrieved by merchant ID.");

        public async Task<OrderItem?> GetById(Guid id)
        {
            var shops = await GetRaw(new GetOrderItemsParameters() { Id = id });
            return shops.ToList().SingleOrDefault();
        }

        public Task<MutationResult> Create(OrderItem item, Guid mutationId)
            => _dataAccessLayer.CreateOrderItem(item, mutationId);

        public Task<MutationResult> Update(OrderItem item, Guid mutationId)
            => _dataAccessLayer.UpdateOrderItem(item, mutationId);

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => _dataAccessLayer.DeleteOrderItem(id, mutationId);

        private async Task<IEnumerable<OrderItem>> GetRaw(GetOrderItemsParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetOrderItems(parameters);
            List<OrderItem> orderItems = [];

            foreach (DataRow row in table.Rows)
            {
                orderItems.Add(new OrderItem()
                {
                    Id = new Guid(row["ori_id"].ToString()!),
                    OrderId = new Guid(row["ori_order"].ToString()!),
                    Type = (OrderItemType)Convert.ToInt32(row["ori_type"]),
                    ProductId = Utilities.DbNullableGuid(row["ori_product"]),
                    Description = Utilities.DbNullableString(row["ori_description"]),
                    Amount = Convert.ToUInt32(row["ori_amount"]),
                    Price = Convert.ToDecimal(row["ori_price"])
                });
            }

            return orderItems;
        }
    }
}