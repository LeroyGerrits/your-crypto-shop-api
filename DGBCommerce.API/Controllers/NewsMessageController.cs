using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGBCommerce.API.Controllers
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

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewsMessage>>> Get()
        {
            IEnumerable<NewsMessage> newsMessages = await _newsMessageRepository.Get();
            return Ok(newsMessages.ToList());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<NewsMessage>> Get(Guid id)
        {
            NewsMessage? newsMessage = await _newsMessageRepository.GetById(id);
            if (newsMessage == null) return NotFound();

            return Ok(newsMessage);
        }
    }
}