using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace DGGCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _shopRepository;

        public CategoryController(ICategoryRepository shopRepository)
        {
            _shopRepository = shopRepository;
        }

        [HttpGet]
        public Task<ActionResult<IEnumerable<Category>>> Get()
            => throw new InvalidOperationException($"A complete list of '{nameof(Category)}' objects may not be retrieved.");

        [HttpGet("{merchantId}")]
        public async Task<ActionResult<IEnumerable<Category>>> GetByMerchantId(Guid merchantId)
        {
            IEnumerable<Category> categories = await _shopRepository.GetByMerchantId(merchantId);
            return Ok(categories.ToList());
        }

        [HttpGet("{parentId}")]
        public async Task<ActionResult<IEnumerable<Category>>> GetByParentId(Guid parentId)
        {
            IEnumerable<Category> categories = await _shopRepository.GetByParentId(parentId);
            return Ok(categories.ToList());
        }

        [HttpGet("{shopId}")]
        public async Task<ActionResult<IEnumerable<Category>>> GetByShopId(Guid shopId)
        {
            IEnumerable<Category> categories = await _shopRepository.GetByMerchantId(shopId);
            return Ok(categories.ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> Get(Guid id)
        {
            Category category = await _shopRepository.GetById(id);
            if (category == null) return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Category value)
        {
            var result = await _shopRepository.Insert(value);
            return CreatedAtAction(nameof(Get), new { id = result.Identifier });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] Category value)
        {
            Category category = await _shopRepository.GetById(id);
            if (category == null) return NotFound();

            var result = await _shopRepository.Update(value);
            if (result.ErrorCode > 0)
                return NoContent();

            return Ok(category);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> Delete(Guid id)
        {
            Category category = await _shopRepository.GetById(id);
            if (category == null) return NotFound();

            var result = await _shopRepository.Delete(id);
            if (result.ErrorCode > 0)
                return NoContent();

            return Ok(category);
        }
    }
}