using DGBCommerce.Domain.Models;

namespace DGBCommerce.API.Controllers.Requests
{
    public class MutatePageRequest
    {
        public required Page Page { get; set; }
        public required string? CheckedCategories { get; set; }
    }
}