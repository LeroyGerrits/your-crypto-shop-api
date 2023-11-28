using DGBCommerce.Domain.Models;

namespace DGBCommerce.API.Controllers.Requests
{
    public class MutateCustomerRequest
    {
        public required Customer Customer { get; set; }
        public required string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public required string PostalCode { get; set; }
        public required string City { get; set; }
        public string? Province { get; set; }
        public required Country Country { get; set; }
    }
}