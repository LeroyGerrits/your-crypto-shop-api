using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FaqCategoryController(IFaqCategoryRepository faqCategoryRepository) : ControllerBase
    {
        private readonly IFaqCategoryRepository _faqCategoryRepository = faqCategoryRepository;

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FaqCategory>>> Get(string? name)
        {
            var faqCategories = await _faqCategoryRepository.Get(new GetFaqCategoriesParameters() { Name = name });
            return Ok(faqCategories.ToList());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<FaqCategory>> Get(Guid id)
        {
            var faqCategory = await _faqCategoryRepository.GetById(id);
            if (faqCategory == null)
                return NotFound();

            return Ok(faqCategory);
        }
    }
}