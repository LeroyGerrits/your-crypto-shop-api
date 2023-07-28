using DGBCommerce.Domain.Enums;

namespace DGBCommerce.Domain.Models
{
    public class Merchant
    {
        public Guid? Id { get; set; }
        public required string EmailAddress { get; set; }
        public required string Password { get; set; }
        public required Gender Gender { get; set; }
        public string? FirstName { get; set; }
        public required string LastName { get; set; }
    }
}