using DGBCommerce.API.Controllers.Attributes;
using DGBCommerce.API.Controllers.Requests;
using DGBCommerce.API.Controllers.Responses;
using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController(IHttpContextAccessor httpContextAccessor, IJwtUtils jwtUtils, ICategoryRepository categoryRepository, IProductRepository productRepository, IProduct2CategoryRepository product2CategoryRepository) : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IJwtUtils _jwtUtils = jwtUtils;
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IProduct2CategoryRepository _product2CategoryRepository = product2CategoryRepository;

        [AuthenticationRequired]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get(string? name, Guid? shopId, Guid? categoryId, bool? visible, bool? showOnHome)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var products = await _productRepository.Get(new GetProductsParameters()
            {
                MerchantId = authenticatedMerchantId.Value,
                Name = name,
                ShopId = shopId,
                CategoryId = categoryId,
                Visible = visible,
                ShowOnHome = showOnHome

            });
            return Ok(products.ToList());
        }

        [AuthenticationRequired]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetProductResponse>> Get(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var product = await _productRepository.GetById(authenticatedMerchantId.Value, id);
            if (product == null)
                return NotFound();

            var product2Categories = await _product2CategoryRepository.Get(new GetProduct2CategoriesParameters() { MerchantId = authenticatedMerchantId.Value, ProductId = product.Id });
            var selectedCategoryIds = product2Categories.Select(c => c.CategoryId).ToList();

            return Ok(new GetProductResponse(product, selectedCategoryIds));
        }

        [AllowAnonymous]
        [HttpGet("public")]
        public async Task<ActionResult<IEnumerable<PublicProduct>>> GetPublic(Guid shopId, string? name, Guid? categoryId, bool? visible, bool? showOnHome)
        {
            var products = await _productRepository.GetPublic(new GetProductsParameters()
            {
                Name = name,
                ShopId = shopId,
                CategoryId = categoryId,
                Visible = visible,
                ShowOnHome = showOnHome
            });

            return Ok(products.ToList());
        }

        [AllowAnonymous]
        [HttpGet("public/{id}")]
        public async Task<ActionResult<IEnumerable<PublicProduct>>> GetPublicById(Guid shopId, Guid id)
        {
            var product = await _productRepository.GetByIdPublic(shopId, id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [AuthenticationRequired]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MutateProductRequest value)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var result = await _productRepository.Create(value.Product, authenticatedMerchantId.Value);

            if (result.Success)
                this.ProcessCheckedCategories(authenticatedMerchantId.Value, result.Identifier, value.CheckedCategories);

            return Ok(result);
        }

        [AuthenticationRequired]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] MutateProductRequest value)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var product = await _productRepository.GetById(authenticatedMerchantId.Value, id);
            if (product == null)
                return NotFound();

            var result = await _productRepository.Update(value.Product, authenticatedMerchantId.Value);

            if (result.Success)
                this.ProcessCheckedCategories(authenticatedMerchantId.Value, id, value.CheckedCategories);

            return Ok(result);
        }

        [AuthenticationRequired]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> Delete(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var product = await _productRepository.GetById(authenticatedMerchantId.Value, id);
            if (product == null)
                return NotFound();

            var result = await _productRepository.Delete(id, authenticatedMerchantId.Value);
            return Ok(result);
        }

        private async void ProcessCheckedCategories(Guid merchantId, Guid productId, string? checkedCategoriesString)
        {
            if (!string.IsNullOrWhiteSpace(checkedCategoriesString))
            {
                string[] splitCheckedCategoriesString = checkedCategoriesString.Split(',');
                if (splitCheckedCategoriesString.Length > 0)
                {
                    List<Guid> checkedCategoryIds = [];

                    foreach (string checkedCategoryIdString in splitCheckedCategoriesString)
                    {
                        Guid? checkedCategoryId = Utilities.NullableGuid(checkedCategoryIdString);
                        if (checkedCategoryId.HasValue)
                            checkedCategoryIds.Add(checkedCategoryId.Value);
                    }

                    var categories = await _categoryRepository.Get(new GetCategoriesParameters() { MerchantId = merchantId });
                    foreach (Category category in categories)
                    {
                        if (checkedCategoryIds.Contains(category.Id!.Value))
                            await _product2CategoryRepository.Create(new() { ProductId = productId, CategoryId = category.Id.Value }, merchantId);
                        else
                            await _product2CategoryRepository.Delete(productId, category.Id.Value, merchantId);
                    }
                }
            }
        }
    }
}