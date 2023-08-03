using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace DGGCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FaqCategoryController : ControllerBase
    {
        private readonly IFaqCategoryRepository _faqCategoryRepository;

        public FaqCategoryController(IFaqCategoryRepository faqCategoryRepository)
        {
            _faqCategoryRepository = faqCategoryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FaqCategory>>> Get()
        {
            IEnumerable<FaqCategory> faqCategories = await _faqCategoryRepository.Get();
            return Ok(faqCategories.ToList());
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<FaqCategory>> Get(Guid id)
        {
            FaqCategory faqCategory = await _faqCategoryRepository.GetById(id);
            if (faqCategory == null) return NotFound();

            return Ok(faqCategory);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] FaqCategory value)
        {
            var result = await _faqCategoryRepository.Insert(value);
            return CreatedAtAction(nameof(Get), new { id = result.Identifier });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] FaqCategory value)
        {
            FaqCategory faqCategory = await _faqCategoryRepository.GetById(id);
            if (faqCategory == null) return NotFound();

            var result = await _faqCategoryRepository.Update(value);
            if (result.ErrorCode > 0)
                return NoContent();

            return Ok(faqCategory);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<FaqCategory>> Delete(Guid id)
        {
            FaqCategory faqCategory = await _faqCategoryRepository.GetById(id);
            if (faqCategory == null) return NotFound();

            var result = await _faqCategoryRepository.Delete(id);
            if (result.ErrorCode > 0)
                return NoContent();

            return Ok(faqCategory);
        }
    }
}