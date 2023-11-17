using DGBCommerce.API.Controllers.Attributes;
using DGBCommerce.API.Controllers.Requests;
using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtUtils _jwtUtils;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProduct2CategoryRepository _product2CategoryRepository;

        public ProductController(
            IHttpContextAccessor httpContextAccessor,
            IJwtUtils jwtUtils,
            ICategoryRepository categoryRepository,
            IProductRepository productMethodRepository,
            IProduct2CategoryRepository product2CategoryRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _jwtUtils = jwtUtils;
            _categoryRepository = categoryRepository;
            _productRepository = productMethodRepository;
            _product2CategoryRepository = product2CategoryRepository;
        }

        [AuthenticationRequired]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get(string? name, Guid? shopId, Guid? categoryId)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var products = await _productRepository.Get(new GetProductsParameters()
            {
                MerchantId = authenticatedMerchantId.Value,
                Name = name,
                ShopId = shopId,
                CategoryId = categoryId
            });
            return Ok(products.ToList());
        }

        [AuthenticationRequired]
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var product = await _productRepository.GetById(authenticatedMerchantId.Value, id);
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
                    List<Guid> checkedCategoryIds = new();

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