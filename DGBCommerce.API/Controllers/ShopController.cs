using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace DGGCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShopController : ControllerBase
    {
        private readonly IShopRepository _shopRepository;

        public ShopController(IShopRepository shopRepository)
        {
            _shopRepository = shopRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shop>>> Get()
        {
            IEnumerable<Shop> shops = await _shopRepository.Get();
            return Ok(shops.ToList());
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Shop>> Get(Guid id)
        {
            Shop shop = await _shopRepository.GetById(id);
            if (shop == null) return NotFound();

            return Ok(shop);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Shop value)
        {
            var result = await _shopRepository.Insert(value);
            return CreatedAtAction(nameof(Get), new { id = result.Identifier });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] Shop value)
        {
            Shop shop = await _shopRepository.GetById(id);
            if (shop == null) return NotFound();

            var result = await _shopRepository.Update(value);
            if (result.ErrorCode > 0)
                return NoContent();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Shop>> Delete(Guid id)
        {
            Shop shop = await _shopRepository.GetById(id);
            if (shop == null) return NotFound();

            var result = await _shopRepository.Delete(id);
            if (result.ErrorCode > 0)
                return NoContent();

            return Ok(shop);
        }
    }
}