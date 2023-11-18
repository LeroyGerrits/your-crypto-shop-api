using DGBCommerce.Domain.Models;

namespace DGBCommerce.API.Controllers.Responses
{
    public class GetProductResponse
    {
        public Product Product { get; set; }
        public List<Guid> CategoryIds { get; set; }

        public GetProductResponse(Product product, List<Guid> categoryIds)
        {
            Product = product;
            CategoryIds = categoryIds;
        }
    }
}