using YourCryptoShop.Domain.Models;

namespace YourCryptoShop.API.Controllers.Requests
{
    public class MutateProductRequest
    {
        public required Product Product { get; set; }
        public required string? CheckedCategories { get; set; }
        public Dictionary<Guid, string?>? FieldData { get; set; }
    }
}