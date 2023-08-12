using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGBCommerce.API.Controllers
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

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FaqCategory>>> Get()
        {
            var faqCategories = await _faqCategoryRepository.Get();
            return Ok(faqCategories.ToList());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<FaqCategory>> Get(Guid id)
        {
            var faqCategory = await _faqCategoryRepository.GetById(id);
            if (faqCategory == null) return NotFound();

            return Ok(faqCategory);
        }
    }
}