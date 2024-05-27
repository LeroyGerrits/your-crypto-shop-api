using YourCryptoShop.Domain.Interfaces.Repositories;
using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YourCryptoShop.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsMessageController(INewsMessageRepository newsMessageRepository) : ControllerBase
    {
        private readonly INewsMessageRepository _newsMessageRepository = newsMessageRepository;

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewsMessage>>> Get(string? title, DateTime? dateFrom, DateTime? dateUntil)
        {
            var newsMessages = await _newsMessageRepository.Get(new GetNewsMessagesParameters()
            {
                Title = title,
                DateFrom = dateFrom,
                DateUntil = dateUntil
            });
            return Ok(newsMessages.ToList());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<NewsMessage>> Get(Guid id)
        {
            var newsMessage = await _newsMessageRepository.GetById(id);
            if (newsMessage == null)
                return NotFound();

            return Ok(newsMessage);
        }
    }
}