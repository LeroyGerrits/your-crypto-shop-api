using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace DGGCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsMessageController : ControllerBase
    {
        private readonly INewsMessageRepository _newsMessageRepository;

        public NewsMessageController(INewsMessageRepository newsMessageRepository)
        {
            _newsMessageRepository = newsMessageRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewsMessage>>> Get()
        {
            IEnumerable<NewsMessage> newsMessages = await _newsMessageRepository.Get();
            return Ok(newsMessages.ToList());
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<NewsMessage>> Get(Guid id)
        {
            NewsMessage newsMessage = await _newsMessageRepository.GetById(id);
            if (newsMessage == null) return NotFound();

            return Ok(newsMessage);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] NewsMessage value)
        {
            var result = await _newsMessageRepository.Insert(value);
            return CreatedAtAction(nameof(Get), new { id = result.Identifier });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] NewsMessage value)
        {
            NewsMessage newsMessage = await _newsMessageRepository.GetById(id);
            if (newsMessage == null) return NotFound();

            var result = await _newsMessageRepository.Update(value);
            if (result.ErrorCode > 0)
                return NoContent();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<NewsMessage>> Delete(Guid id)
        {
            NewsMessage newsMessage = await _newsMessageRepository.GetById(id);
            if (newsMessage == null) return NotFound();

            var result = await _newsMessageRepository.Delete(id);

            if (result.ErrorCode > 0)
                return NoContent();

            return Ok(newsMessage);
        }
    }
}