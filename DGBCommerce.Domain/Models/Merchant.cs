using DGBCommerce.Domain.Enums;
using System.Text.Json.Serialization;

namespace DGBCommerce.Domain.Models
{
    public class Merchant
    {
        public Guid? Id { get; set; }
        public required string EmailAddress { get; set; }
        [JsonIgnore] public string? PasswordSalt { get; set; }
        [JsonIgnore] public string? Password { get; set; }
        public required Gender Gender { get; set; }
        public string? FirstName { get; set; }
        public required string LastName { get; set; }

        public string Salutation
            => this.Gender switch
            {
                Gender.Male => "Mr." + (!string.IsNullOrWhiteSpace(this.FirstName) ? " " + this.FirstName : string.Empty) + " " + this.LastName,
                Gender.Female => "Ms. " + (!string.IsNullOrWhiteSpace(this.FirstName) ? " " + this.FirstName : string.Empty) + " " + this.LastName,
                _ => "Mr./Ms. " + (!string.IsNullOrWhiteSpace(this.FirstName) ? " " + this.FirstName : string.Empty) + " " + this.LastName,
            };
    }
}