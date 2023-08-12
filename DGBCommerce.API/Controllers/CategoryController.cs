using DGBCommerce.API.Controllers.Attributes;
using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtUtils _jwtUtils;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(
            ILogger<CategoryController> logger,
            IHttpContextAccessor httpContextAccessor,
            IJwtUtils jwtUtils,
            ICategoryRepository shopRepository
            )
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _jwtUtils = jwtUtils;
            _categoryRepository = shopRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public Task<ActionResult<IEnumerable<Category>>> Get()
            => throw new InvalidOperationException($"A complete list of '{nameof(Category)}' objects may not be retrieved.");

        [AllowAnonymous]
        [HttpGet("{merchantId}")]
        public async Task<ActionResult<IEnumerable<Category>>> GetByMerchantId(Guid merchantId)
        {
            IEnumerable<Category> categories = await _categoryRepository.GetByMerchantId(merchantId);
            return Ok(categories.ToList());
        }

        [AllowAnonymous]
        [HttpGet("{parentId}")]
        public async Task<ActionResult<IEnumerable<Category>>> GetByParentId(Guid parentId)
        {
            IEnumerable<Category> categories = await _categoryRepository.GetByParentId(parentId);
            return Ok(categories.ToList());
        }

        [AllowAnonymous]
        [HttpGet("{shopId}")]
        public async Task<ActionResult<IEnumerable<Category>>> GetByShopId(Guid shopId)
        {
            IEnumerable<Category> categories = await _categoryRepository.GetByMerchantId(shopId);
            return Ok(categories.ToList());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> Get(Guid id)
        {
            Category? category = await _categoryRepository.GetById(id);
            if (category == null) return NotFound();

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

            var category = await _categoryRepository.GetById(id);
            if (category == null) return NotFound();

            var result = await _categoryRepository.Update(value, authenticatedMerchantId.Value);
            if (result.ErrorCode > 0)
                return NoContent();

            return Ok(category);
        }

        [AuthenticationRequired]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> Delete(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var category = await _categoryRepository.GetById(id);
            if (category == null) return NotFound();

            var result = await _categoryRepository.Delete(id, authenticatedMerchantId.Value);
            if (result.ErrorCode > 0)
                return NoContent();

            return Ok(category);
        }
    }
}