using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace DGGCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FaqController : ControllerBase
    {
        private readonly IFaqRepository _shopRepository;

        public FaqController(IFaqRepository shopRepository)
        {
            _shopRepository = shopRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Faq>>> Get()
        {
            IEnumerable<Faq> shops = await _shopRepository.Get();
            return Ok(shops.ToList());
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Faq>> Get(Guid id)
        {
            Faq shop = await _shopRepository.GetById(id);
            if (shop == null) return NotFound();

            return Ok(shop);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Faq value)
        {
            var result = await _shopRepository.Insert(value);
            return CreatedAtAction(nameof(Get), new { id = value.Id });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] Faq value)
        {
            Faq shop = await _shopRepository.GetById(id);
            if (shop == null) return NotFound();

            var result = await _shopRepository.Update(value);
            if (result.ErrorCode > 0)
                return NoContent();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Faq>> Delete(Guid id)
        {
            Faq shop = await _shopRepository.GetById(id);
            if (shop == null) return NotFound();

            var result = await _shopRepository.Delete(id);

            if (result.ErrorCode > 0)
                return NoContent();

            return Ok(shop);
        }
    }
}