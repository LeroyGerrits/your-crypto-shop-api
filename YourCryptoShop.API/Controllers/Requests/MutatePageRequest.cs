using YourCryptoShop.Domain.Models;

namespace YourCryptoShop.API.Controllers.Requests
{
    public class MutatePageRequest
    {
        public required Page Page { get; set; }
        public required string? CheckedCategories { get; set; }
    }
}