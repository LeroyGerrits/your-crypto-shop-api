using YourCryptoShop.Domain.Interfaces.Repositories;
using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YourCryptoShop.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PageCategoryController(IPageCategoryRepository currencyRepository) : ControllerBase
    {
        private readonly IPageCategoryRepository _pageCategoryRepository = currencyRepository;

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PageCategory>>> Get(string? name)
        {
            var pageCategories = await _pageCategoryRepository.Get(new GetPageCategoriesParameters() { Name = name });
            return Ok(pageCategories.ToList());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<PageCategory>> GetById(Guid id)
        {
            var pageCategory = await _pageCategoryRepository.GetById(id);
            if (pageCategory == null)
                return NotFound();

            return Ok(pageCategory);
        }
    }
}