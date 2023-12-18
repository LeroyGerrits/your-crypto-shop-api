using DGBCommerce.Domain.Models;

namespace DGBCommerce.API.Controllers.Responses
{
    public class GetProductResponse(Product product, List<Guid> categoryIds)
    {
        public Product Product { get; set; } = product;
        public List<Guid> CategoryIds { get; set; } = categoryIds;
    }
}