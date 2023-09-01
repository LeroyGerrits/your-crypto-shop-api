using DGBCommerce.API.Controllers.Attributes;
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
            return Ok(categories.ToList());
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
            return CreatedAtAction(nameof(Get), new { id = result.Identifier });
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
            if (result.ErrorCode > 0)
                return Ok(result.Message);

            return Ok(category);
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
            if (result.ErrorCode > 0)
                return Ok(result.Message);

            return Ok(category);
        }
    }
}