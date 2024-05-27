using YourCryptoShop.Domain.Models;

namespace YourCryptoShop.API.Controllers.Responses
{
    public class GetPageResponse(Page page, List<Guid> categoryIds)
    {
        public Page Page { get; set; } = page;
        public List<Guid> CategoryIds { get; set; } = categoryIds;
    }
}