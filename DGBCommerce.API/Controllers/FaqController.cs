using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FaqController : ControllerBase
    {
        private readonly IFaqRepository _faqRepository;

        public FaqController(IFaqRepository faqRepository)
        {
            _faqRepository = faqRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Faq>>> Get()
        {
            var faqs = await _faqRepository.Get();
            return Ok(faqs.ToList());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Faq>> Get(Guid id)
        {
            var faq = await _faqRepository.GetById(id);
            if (faq == null) return NotFound();

            return Ok(faq);
        }
    }
}