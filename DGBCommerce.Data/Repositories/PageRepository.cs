using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Models.ViewModels;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Data.Repositories
{
    public class PageRepository(IDataAccessLayer dataAccessLayer) : IPageRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<Page>> Get(GetPagesParameters parameters)
            => await GetRaw(parameters);

        public async Task<Page?> GetById(Guid merchantId, Guid id)
        {
            var pages = await GetRaw(new GetPagesParameters() { MerchantId = merchantId, Id = id });
            return pages.ToList().SingleOrDefault();
        }

        public async Task<IEnumerable<PublicPage>> GetByShopIdPublic(Guid shopId)
        {
            var pages = await GetRawPublic(new GetPagesParameters() { ShopId = shopId });
            return pages.ToList();
        }

        public Task<MutationResult> Create(Page item, Guid mutationId)
            => _dataAccessLayer.CreatePage(item, mutationId);

        public Task<MutationResult> Update(Page item, Guid mutationId)
            => _dataAccessLayer.UpdatePage(item, mutationId);

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => _dataAccessLayer.DeletePage(id, mutationId);

        private async Task<IEnumerable<Page>> GetRaw(GetPagesParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetPages(parameters);
            List<Page> pages = [];

            foreach (DataRow row in table.Rows)
            {
                pages.Add(new()
                {
                    Id = new Guid(row["pag_id"].ToString()!),
                    Shop = new Shop()
                    {
                        Id = new Guid(row["pag_shop"].ToString()!),
                        Name = Utilities.DbNullableString(row["pag_shop_name"]),
                        MerchantId = new Guid(row["pag_shop_merchant"].ToString()!)
                    },
                    Title = Utilities.DbNullableString(row["pag_title"]),
                    Content = Utilities.DbNullableString(row["pag_content"]),
                    Visible = Convert.ToBoolean(row["pag_visible"]),
                    Index = Convert.ToBoolean(row["pag_index"])
                });
            }

            return pages;
        }

        private async Task<IEnumerable<PublicPage>> GetRawPublic(GetPagesParameters parameters)
        {
            // Only get visible pages
            parameters.Visible = true;

            DataTable table = await _dataAccessLayer.GetPages(parameters);
            List<PublicPage> pages = [];

            foreach (DataRow row in table.Rows)
            {
                pages.Add(new()
                {
                    Id = new Guid(row["pag_id"].ToString()!),
                    Title = Utilities.DbNullableString(row["pag_title"]),
                    Content = Utilities.DbNullableString(row["pag_content"]),
                    Index = Convert.ToBoolean(row["pag_index"]),
                });
            }

            return pages;
        }
    }
}