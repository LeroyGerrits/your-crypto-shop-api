using DGBCommerce.Domain.Enums;

namespace DGBCommerce.Domain.Models.ViewModels
{
    public class PublicMerchant
    {
        public Guid? Id { get; set; }
        public required string Username { get; set; }
        public required Gender Gender { get; set; }
        public string? FirstName { get; set; }
        public required string LastName { get; set; }
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