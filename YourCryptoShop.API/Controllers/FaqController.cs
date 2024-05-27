using YourCryptoShop.Domain.Interfaces.Repositories;
using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YourCryptoShop.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FaqController(IFaqRepository faqRepository) : ControllerBase
    {
        private readonly IFaqRepository _faqRepository = faqRepository;

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Faq>>> Get(string? title, string? keywords)
        {
            var faqs = await _faqRepository.Get(new GetFaqsParameters()
            {
                Title = title,
                Keywords = keywords
            });
            return Ok(faqs.ToList());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Faq>> Get(Guid id)
        {
            var faq = await _faqRepository.GetById(id);
            if (faq == null)
                return NotFound();

            return Ok(faq);
        }
    }
}