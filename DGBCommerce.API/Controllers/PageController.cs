using DGBCommerce.API.Controllers.Attributes;
using DGBCommerce.API.Controllers.Requests;
using DGBCommerce.API.Controllers.Responses;
using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Models.ViewModels;
using DGBCommerce.Domain.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGBCommerce.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PageController(IHttpContextAccessor httpContextAccessor, IJwtUtils jwtUtils, IPageRepository pageRepository, IPageCategoryRepository pageCategoryRepository, IPage2CategoryRepository page2CategoryRepository, IShopRepository shopRepository) : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IJwtUtils _jwtUtils = jwtUtils;
        private readonly IPageRepository _pageRepository = pageRepository;
        private readonly IPageCategoryRepository _pageCategoryRepository = pageCategoryRepository;
        private readonly IPage2CategoryRepository _page2CategoryRepository = page2CategoryRepository;
        private readonly IShopRepository _shopRepository = shopRepository;

        [MerchantAuthenticationRequired]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Page>>> Get(string? title, Guid? shopId, bool? visible)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var pages = await _pageRepository.Get(new GetPagesParameters()
            {
                MerchantId = authenticatedMerchantId.Value,
                Title = title,
                ShopId = shopId,
                Visible = visible
            });
            return Ok(pages.ToList());
        }

        [MerchantAuthenticationRequired]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetPageResponse>> Get(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var page = await _pageRepository.GetById(authenticatedMerchantId.Value, id);
            if (page == null)
                return NotFound();

            var page2Categories = await _page2CategoryRepository.Get(new GetPage2CategoriesParameters() { MerchantId = authenticatedMerchantId.Value, PageId = page.Id });
            var selectedCategoryIds = page2Categories.Select(c => c.CategoryId).ToList();

            return Ok(new GetPageResponse(page, selectedCategoryIds));
        }

        [AllowAnonymous]
        [HttpGet("public/GetByShopId/{shopId}")]
        public async Task<ActionResult<PublicPage>> GetByShopIdPublic(Guid shopId)
        {
            var shop = await _shopRepository.GetByIdPublic(shopId);
            if (shop == null)
                return NotFound();

            var pages = await _pageRepository.GetByShopIdPublic(shopId);
            var page2categories = await _page2CategoryRepository.Get(new GetPage2CategoriesParameters() { MerchantId = shop.MerchantId });

            Dictionary<Guid, List<Guid>> dictCategoryIdsPerPage = [];
            foreach (Page2Category page2category in page2categories)
            {
                if (dictCategoryIdsPerPage.TryGetValue(page2category.PageId, out List<Guid>? value))
                    value.Add(page2category.CategoryId);
                else
                    dictCategoryIdsPerPage.Add(page2category.PageId, [page2category.CategoryId]);
            }

            foreach (PublicPage page in pages)
                if (dictCategoryIdsPerPage.TryGetValue(page.Id!.Value, out List<Guid>? value))
                    page.CategoryIds = value;

            return Ok(pages.ToList());
        }

        [MerchantAuthenticationRequired]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MutatePageRequest value)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var result = await _pageRepository.Create(value.Page, authenticatedMerchantId.Value);

            if (result.Success)
                this.ProcessCheckedCategories(authenticatedMerchantId.Value, result.Identifier, value.CheckedCategories);

            return Ok(result);
        }

        [MerchantAuthenticationRequired]
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] MutatePageRequest value)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var page = await _pageRepository.GetById(authenticatedMerchantId.Value, id);
            if (page == null)
                return NotFound();

            var result = await _pageRepository.Update(value.Page, authenticatedMerchantId.Value);

            if (result.Success)
                this.ProcessCheckedCategories(authenticatedMerchantId.Value, id, value.CheckedCategories);

            return Ok(result);
        }

        [MerchantAuthenticationRequired]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Page>> Delete(Guid id)
        {
            var authenticatedMerchantId = _jwtUtils.GetMerchantId(_httpContextAccessor);
            if (authenticatedMerchantId == null)
                return BadRequest("Merchant not authorized.");

            var page = await _pageRepository.GetById(authenticatedMerchantId.Value, id);
            if (page == null)
                return NotFound();

            var result = await _pageRepository.Delete(id, authenticatedMerchantId.Value);
            return Ok(result);
        }

        private async void ProcessCheckedCategories(Guid merchantId, Guid pageId, string? checkedCategoriesString)
        {
            if (!string.IsNullOrWhiteSpace(checkedCategoriesString))
            {
                string[] splitCheckedCategoriesString = checkedCategoriesString.Split(',');
                if (splitCheckedCategoriesString.Length > 0)
                {
                    List<Guid> checkedCategoryIds = [];

                    foreach (string checkedCategoryIdString in splitCheckedCategoriesString)
                    {
                        Guid? checkedCategoryId = Utilities.NullableGuid(checkedCategoryIdString);
                        if (checkedCategoryId.HasValue)
                            checkedCategoryIds.Add(checkedCategoryId.Value);
                    }

                    var categories = await _pageCategoryRepository.Get(new GetPageCategoriesParameters() { });
                    foreach (PageCategory category in categories)
                    {
                        if (checkedCategoryIds.Contains(category.Id!.Value))
                            await _page2CategoryRepository.Create(new() { PageId = pageId, CategoryId = category.Id.Value }, merchantId);
                        else
                            await _page2CategoryRepository.Delete(pageId, category.Id.Value, merchantId);
                    }
                }
            }
        }
    }
}