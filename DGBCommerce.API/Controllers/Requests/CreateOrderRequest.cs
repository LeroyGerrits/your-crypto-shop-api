using DGBCommerce.Domain.Enums;

namespace DGBCommerce.API.Controllers.Requests
{
    public class CreateOrderRequest
    {
        public required Guid ShopId { get; set; }
        public Guid? CustomerId { get; set; }
        public required string EmailAddress { get; set; }
        public required Gender Gender { get; set; }
        public string? FirstName { get; set; }
        public required string LastName { get; set; }
        public required string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public required string PostalCode { get; set; }
        public required string City { get; set; }
        public string? Province { get; set; }
        public required Guid CountryId { get; set; }
        public required Guid DeliveryMethodId { get; set; }
        public string? Comments { get; set; }
        public required Guid SessionId { get; set; }
    }
}