using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShopCategoryController(IShopCategoryRepository currencyRepository) : ControllerBase
    {
        private readonly IShopCategoryRepository _shopCategoryRepository = currencyRepository;

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShopCategory>>> Get(string? name)
        {
            var shopCategories = await _shopCategoryRepository.Get(new GetShopCategoriesParameters() { Name = name });
            return Ok(shopCategories.ToList());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<ShopCategory>> GetById(Guid id)
        {
            var shopCategory = await _shopCategoryRepository.GetById(id);
            if (shopCategory == null)
                return NotFound();

            return Ok(shopCategory);
        }
    }
}