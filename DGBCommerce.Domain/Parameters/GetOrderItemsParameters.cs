﻿using DGBCommerce.Domain.Enums;

namespace DGBCommerce.Domain.Parameters
{
    public class GetOrderItemsParameters : GetParameters
    {
        public Guid? Id { get; set; }
        public Guid? OrderId { get; set; }
        public OrderItemType? Type { get; set; }
        public Guid? ProductId { get; set; }
        public string? Description { get; set; }
    }
}