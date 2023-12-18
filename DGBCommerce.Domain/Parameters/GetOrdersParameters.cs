using DGBCommerce.Domain.Enums;

namespace DGBCommerce.Domain.Parameters
{
    public class GetOrdersParameters : GetParameters
    {
        public Guid MerchantId { get; set; }
        public Guid? Id { get; set; }        
        public Guid? ShopId { get; set; }        
        public DateTime? DateFrom { get; set; }
        public DateTime? DateUntil { get; set; }
        public OrderStatus? Status { get; set; }
    }
}