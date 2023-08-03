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
        public async Task<ActionResult<IEnumerable<Category>>> Get()
        {
            IEnumerable<Category> categories = await _shopRepository.Get();
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