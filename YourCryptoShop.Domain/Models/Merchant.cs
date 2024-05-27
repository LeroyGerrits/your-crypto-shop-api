using YourCryptoShop.Domain.Enums;
using System.Text.Json.Serialization;

namespace YourCryptoShop.Domain.Models
{
    public class Merchant
    {
        public Guid? Id { get; set; }
        public DateTime? Activated { get; set; }
        public required string EmailAddress { get; set; }
        public required string Username { get; set; }
        [JsonIgnore] public string? PasswordSalt { get; set; }
        [JsonIgnore] public string? Password { get; set; }
        public required Gender Gender { get; set; }
        public string? FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime? LastLogin { get; set; }
        public string? LastIpAddress { get; set; }
        public DateTime? SecondLastLogin { get; set; }
        public string? SecondLastIpAddress { get; set; }
        public decimal? Score { get; set; }

        public string Salutation
            => this.Gender switch
            {
                Gender.Male => (!string.IsNullOrWhiteSpace(this.FirstName) ? this.FirstName : "Mr.") + " " + this.LastName,
                Gender.Female => (!string.IsNullOrWhiteSpace(this.FirstName) ? this.FirstName : "Ms.") + " " + this.LastName,
                _ => (!string.IsNullOrWhiteSpace(this.FirstName) ? this.FirstName : "Mr./Ms.") + " " + this.LastName,
            };
    }
}