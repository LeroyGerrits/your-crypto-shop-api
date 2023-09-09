using DGBCommerce.API.Controllers.Attributes;
using DGBCommerce.Data.Repositories;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using Microsoft.AspNetCore.Mvc;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtUtils _jwtUtils;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(
            IHttpContextAccessor httpContextAccessor,
            IJwtUtils jwtUtils,
            ICategoryRepository shopRepository
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _jwtUtils = jwtUtils;
            _categoryRepository = shopRepository;
        }

        [AuthenticationRequired]
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

            Dictionary<Guid, List<Category>> dictCategoriesPerParent = new();
            foreach (Category category in categories)
            {
                Guid dictionaryKey = category.Parent != null ? category.Parent.Id!.Value : Guid.Empty;

                if (dictCategoriesPerParent.ContainsKey(dictionaryKey))
                    dictCategoriesPerParent[dictionaryKey].Add(category);
                else
                    dictCategoriesPerParent.Add(dictionaryKey, new List<Category> { category });
            }

            foreach (Category category in categories)
                if (dictCategoriesPerParent.ContainsKey(category.Id!.Value))
                    category.Children = dictCategoriesPerParent[category.Id!.Value];

            return Ok(categories.Where(c => c.Parent == null).ToList());
        }

        [AuthenticationRequired]
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

        [AuthenticationRequired]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Category value)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var result = await _categoryRepository.Create(value, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [AuthenticationRequired]
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

        [AuthenticationRequired]
        [HttpPut("{id}/Down")]
        public async Task<ActionResult> MoveDown(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var category = await _categoryRepository.GetById(authenticatedMerchantId.Value, id);
            if (category == null)
                return NotFound();

            var result = await _categoryRepository.MoveDown(id, category.Parent?.Id, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [AuthenticationRequired]
        [HttpPut("{id}/Up")]
        public async Task<ActionResult> MoveUp(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var category = await _categoryRepository.GetById(authenticatedMerchantId.Value, id);
            if (category == null)
                return NotFound();

            var result = await _categoryRepository.MoveUp(id, category.Parent?.Id, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [AuthenticationRequired]
        [HttpPut("{id}/ChangeParent/{parentId}")]
        public async Task<ActionResult> ChangeParent(Guid id, Guid parentId)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var category = await _categoryRepository.GetById(authenticatedMerchantId.Value, id);
            if (category == null)
                return NotFound();

            var result = await _categoryRepository.ChangeParent(id, parentId, authenticatedMerchantId.Value);
            return Ok(result);
        }

        [AuthenticationRequired]
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