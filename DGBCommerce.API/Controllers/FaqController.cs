using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace DGGCommerce.API.Controllers
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Faq>>> Get()
        {
            IEnumerable<Faq> faqs = await _faqRepository.Get();
            return Ok(faqs.ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Faq>> Get(Guid id)
        {
            Faq faq = await _faqRepository.GetById(id);
            if (faq == null) return NotFound();

            return Ok(faq);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Faq value)
        {
            var result = await _faqRepository.Insert(value);
            return CreatedAtAction(nameof(Get), new { id = result.Identifier });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] Faq value)
        {
            Faq faq = await _faqRepository.GetById(id);
            if (faq == null) return NotFound();

            var result = await _faqRepository.Update(value);
            if (result.ErrorCode > 0)
                return NoContent();

            return Ok(faq);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Faq>> Delete(Guid id)
        {
            Faq faq = await _faqRepository.GetById(id);
            if (faq == null) return NotFound();

            var result = await _faqRepository.Delete(id);
            if (result.ErrorCode > 0)
                return NoContent();

            return Ok(faq);
        }
    }
}