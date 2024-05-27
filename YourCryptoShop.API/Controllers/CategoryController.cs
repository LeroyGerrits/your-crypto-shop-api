using YourCryptoShop.API.Controllers.Attributes;
using YourCryptoShop.Domain;
using YourCryptoShop.Domain.Interfaces.Repositories;
using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Models.ViewModels;
using YourCryptoShop.Domain.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YourCryptoShop.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController(IHttpContextAccessor httpContextAccessor, IJwtUtils jwtUtils, ICategoryRepository shopRepository) : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IJwtUtils _jwtUtils = jwtUtils;
        private readonly ICategoryRepository _categoryRepository = shopRepository;

        [MerchantAuthenticationRequired]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get(Guid? shopId, Guid? parentId, string? name)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var categories = await _categoryRepository.Get(new GetCategoriesParameters()
            {
                MerchantId = authenticatedMerchantId.Value,
                ShopId = shopId,
                ParentId = parentId,
                Name = name
            });

            Dictionary<Guid, List<Category>> dictCategoriesPerParent = [];
            foreach (Category category in categories)
            {
                Guid dictionaryKey = category.ParentId ?? Guid.Empty;

                if (dictCategoriesPerParent.TryGetValue(dictionaryKey, out List<Category>? value))
                    value.Add(category);
                else
                    dictCategoriesPerParent.Add(dictionaryKey, [category]);
            }

            foreach (Category category in categories)
                if (dictCategoriesPerParent.TryGetValue(category.Id!.Value, out List<Category>? value))
                    category.Children = value;

            return Ok(categories.Where(c => !c.ParentId.HasValue).ToList());
        }

        [MerchantAuthenticationRequired]
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetById(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var category = await _categoryRepository.GetById(authenticatedMerchantId.Value, id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [AllowAnonymous]
        [HttpGet("public/GetByShopId/{shopId}")]
        public async Task<ActionResult<PublicCategory>> GetByShopIdPublic(Guid shopId)
        {
            var categories = await _categoryRepository.GetByShopIdPublic(shopId);

            Dictionary<Guid, List<PublicCategory>> dictCategoriesPerParent = [];
            foreach (PublicCategory category in categories)
            {
                Guid dictionaryKey = category.ParentId ?? Guid.Empty;

                if (dictCategoriesPerParent.TryGetValue(dictionaryKey, out List<PublicCategory>? value))
                    value.Add(category);
                else
                    dictCategoriesPerParent.Add(dictionaryKey, [category]);
            }

            foreach (PublicCategory category in categories)
                if (dictCategoriesPerParent.TryGetValue(category.Id, out List<PublicCategory>? value))
                    category.Children = value;

            return Ok(categories.Where(c => !c.ParentId.HasValue).ToList());
        }

        [MerchantAuthenticationRequired]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Category value)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var result = await _categoryRepository.Create(value, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [MerchantAuthenticationRequired]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] Category value)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var category = await _categoryRepository.GetById(authenticatedMerchantId.Value, id);
            if (category == null)
                return NotFound();

            var result = await _categoryRepository.Update(value, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [MerchantAuthenticationRequired]
        [HttpPut("{id}/Down")]
        public async Task<ActionResult> MoveDown(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var category = await _categoryRepository.GetById(authenticatedMerchantId.Value, id);
            if (category == null)
                return NotFound();

            var result = await _categoryRepository.MoveDown(id, category.ParentId, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [MerchantAuthenticationRequired]
        [HttpPut("{id}/Up")]
        public async Task<ActionResult> MoveUp(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var category = await _categoryRepository.GetById(authenticatedMerchantId.Value, id);
            if (category == null)
                return NotFound();

            var result = await _categoryRepository.MoveUp(id, category.ParentId, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [MerchantAuthenticationRequired]
        [HttpPut("{id}/ChangeParent/{parentId}")]
        public async Task<ActionResult> ChangeParent(Guid id, Guid parentId)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var category = await _categoryRepository.GetById(authenticatedMerchantId.Value, id);
            if (category == null)
                return NotFound();

            // Check if category was not moved to one of its child categories
            var categories = await _categoryRepository.Get(new GetCategoriesParameters() { MerchantId = authenticatedMerchantId.Value });
            Dictionary<Guid, Category> dictCategories = categories.ToDictionary(cat => cat.Id!.Value);
            Category newParentCategory = dictCategories[parentId];

            if (newParentCategory.IsAChildOfCategory(category, ref dictCategories))
                return Ok(new MutationResult() { ErrorCode = -1, Message = "A new parent can not be a child of the selected category." });

            var result = await _categoryRepository.ChangeParent(id, parentId, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [MerchantAuthenticationRequired]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> Delete(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var category = await _categoryRepository.GetById(authenticatedMerchantId.Value, id);
            if (category == null)
                return NotFound();

            var result = await _categoryRepository.Delete(id, authenticatedMerchantId.Value);
            return Ok(result);
        }
    }
}