using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Data.Repositories
{
    public class FaqCategoryRepository(IDataAccessLayer dataAccessLayer) : IFaqCategoryRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<FaqCategory>> Get(GetFaqCategoriesParameters parameters)
            => await GetRaw(parameters);

        public async Task<FaqCategory?> GetById(Guid id)
        {
            var faqCategories = await GetRaw(new GetFaqCategoriesParameters() { Id = id });
            return faqCategories.ToList().SingleOrDefault();
        }

        private async Task<IEnumerable<FaqCategory>> GetRaw(GetFaqCategoriesParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetFaqCategories(parameters);
            List<FaqCategory> faqcategories = [];

            foreach (DataRow row in table.Rows)
            {
                faqcategories.Add(new()
                {
                    Id = new Guid(row["cat_id"].ToString()!),
                    Name = Utilities.DbNullableString(row["cat_name"]),
                    SortOrder = Utilities.DbNullableInt(row["cat_sortorder"])
                });
            }

            return faqcategories;
        }
    }
}