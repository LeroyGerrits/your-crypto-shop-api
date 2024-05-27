namespace YourCryptoShop.Domain.Models.ViewModels
{
    public class PublicCustomer
    {
        public Guid? Id { get; set; }
        public required string Username { get; set; }
    }
}