using DGBCommerce.API.Controllers.Attributes;
using DGBCommerce.API.Controllers.Requests;
using DGBCommerce.API.Controllers.Responses;
using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Models.ViewModels;
using DGBCommerce.Domain.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController(
        IHttpContextAccessor httpContextAccessor,
        IJwtUtils jwtUtils,
        ICategoryRepository categoryRepository,
        IProductRepository productRepository,
        IProduct2CategoryRepository product2CategoryRepository,
        IProductPhotoRepository productPhotoRepository
        ) : ControllerBase
    {
        [MerchantAuthenticationRequired]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get(string? code, string? name, Guid? shopId, Guid? categoryId, bool? visible, bool? showOnHome)
        {
            var authenticatedMerchantId = jwtUtils.GetMerchantId(httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var products = await productRepository.Get(new GetProductsParameters()
            {
                MerchantId = authenticatedMerchantId.Value,
                Code = code,
                Name = name,
                ShopId = shopId,
                CategoryId = categoryId,
                Visible = visible,
                ShowOnHome = showOnHome

            });
            return Ok(products.ToList());
        }

        [MerchantAuthenticationRequired]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetProductResponse>> Get(Guid id)
        {
            var authenticatedMerchantId = jwtUtils.GetMerchantId(httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var product = await productRepository.GetById(authenticatedMerchantId.Value, id);
            if (product == null)
                return NotFound();

            var product2Categories = await product2CategoryRepository.Get(new GetProduct2CategoriesParameters() { MerchantId = authenticatedMerchantId.Value, ProductId = product.Id });
            var selectedCategoryIds = product2Categories.Select(c => c.CategoryId).ToList();

            return Ok(new GetProductResponse(product, selectedCategoryIds));
        }

        [AllowAnonymous]
        [HttpGet("public")]
        public async Task<ActionResult<IEnumerable<PublicProduct>>> GetPublic(Guid shopId, string? code, string? name, Guid? categoryId, bool? visible, bool? showOnHome)
        {
            var products = await productRepository.GetPublic(new GetProductsParameters()
            {
                Code = code,
                Name = name,
                ShopId = shopId,
                CategoryId = categoryId,
                Visible = visible,
                ShowOnHome = showOnHome
            });

            return Ok(products.ToList());
        }

        [AllowAnonymous]
        [HttpGet("public/{shopId}/{id}")]
        public async Task<ActionResult<IEnumerable<PublicProduct>>> GetPublicById(Guid shopId, Guid id)
        {
            var product = await productRepository.GetByIdPublic(shopId, id);
            if (product == null)
                return NotFound();

            var productPhotos = await productPhotoRepository.GetByProductIdPublic(product.Id!.Value);
            product.Photos = productPhotos.ToList();

            return Ok(product);
        }

        [MerchantAuthenticationRequired]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MutateProductRequest value)
        {
            var authenticatedMerchantId = jwtUtils.GetMerchantId(httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var result = await productRepository.Create(value.Product, authenticatedMerchantId.Value);

            if (result.Success)
                this.ProcessCheckedCategories(authenticatedMerchantId.Value, result.Identifier, value.CheckedCategories);

            return Ok(result);
        }

        [MerchantAuthenticationRequired]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] MutateProductRequest value)
        {
            var authenticatedMerchantId = jwtUtils.GetMerchantId(httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var product = await productRepository.GetById(authenticatedMerchantId.Value, id);
            if (product == null)
                return NotFound();

            var result = await productRepository.Update(value.Product, authenticatedMerchantId.Value);

            if (result.Success)
                this.ProcessCheckedCategories(authenticatedMerchantId.Value, id, value.CheckedCategories);

            return Ok(result);
        }

        [MerchantAuthenticationRequired]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> Delete(Guid id)
        {
            var authenticatedMerchantId = jwtUtils.GetMerchantId(httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var product = await productRepository.GetById(authenticatedMerchantId.Value, id);
            if (product == null)
                return NotFound();

            var result = await productRepository.Delete(id, authenticatedMerchantId.Value);
            return Ok(result);
        }

        private async void ProcessCheckedCategories(Guid merchantId, Guid productId, string? checkedCategoriesString)
        {
            List<Guid> checkedCategoryIds = [];

            if (!string.IsNullOrWhiteSpace(checkedCategoriesString))
            {
                string[] splitCheckedCategoriesString = checkedCategoriesString!.Split(',');

                foreach (string checkedCategoryIdString in splitCheckedCategoriesString)
                {
                    Guid? checkedCategoryId = Utilities.NullableGuid(checkedCategoryIdString);
                    if (checkedCategoryId.HasValue)
                        checkedCategoryIds.Add(checkedCategoryId.Value);
                }
            }

            var categories = await categoryRepository.Get(new GetCategoriesParameters() { MerchantId = merchantId });
            foreach (Category category in categories)
            {
                if (checkedCategoryIds.Contains(category.Id!.Value))
                    await product2CategoryRepository.Create(new() { ProductId = productId, CategoryId = category.Id.Value }, merchantId);
                else
                    await product2CategoryRepository.Delete(productId, category.Id.Value, merchantId);
            }
        }
    }
}