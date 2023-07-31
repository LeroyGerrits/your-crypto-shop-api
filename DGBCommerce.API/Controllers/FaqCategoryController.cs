using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace DGGCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FaqCategoryController : ControllerBase
    {
        private readonly IFaqCategoryRepository _shopRepository;

        public FaqCategoryController(IFaqCategoryRepository shopRepository)
        {
            _shopRepository = shopRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FaqCategory>>> Get()
        {
            IEnumerable<FaqCategory> shops = await _shopRepository.Get();
            return Ok(shops.ToList());
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<FaqCategory>> Get(Guid id)
        {
            FaqCategory shop = await _shopRepository.GetById(id);
            if (shop == null) return NotFound();

            return Ok(shop);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] FaqCategory value)
        {
            var result = await _shopRepository.Insert(value);
            return CreatedAtAction(nameof(Get), new { id = value.Id });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] FaqCategory value)
        {
            FaqCategory shop = await _shopRepository.GetById(id);
            if (shop == null) return NotFound();

            var result = await _shopRepository.Update(value);
            if (result.ErrorCode > 0)
                return NoContent();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<FaqCategory>> Delete(Guid id)
        {
            FaqCategory shop = await _shopRepository.GetById(id);
            if (shop == null) return NotFound();

            var result = await _shopRepository.Delete(id);

            if (result.ErrorCode > 0)
                return NoContent();

            return Ok(shop);
        }
    }
}