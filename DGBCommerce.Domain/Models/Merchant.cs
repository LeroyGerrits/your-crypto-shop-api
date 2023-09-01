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
                Gender.Male => (!string.IsNullOrWhiteSpace(this.FirstName) ? this.FirstName : "Mr.") + " " + this.LastName,
                Gender.Female => (!string.IsNullOrWhiteSpace(this.FirstName) ? this.FirstName : "Ms.") + " " + this.LastName,
                _ => (!string.IsNullOrWhiteSpace(this.FirstName) ? this.FirstName : "Mr./Ms.") + " " + this.LastName,
            };
    }
}