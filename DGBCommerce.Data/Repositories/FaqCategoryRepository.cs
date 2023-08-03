using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Data.Repositories
{
    public class FaqCategoryRepository : IFaqCategoryRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer;

        public FaqCategoryRepository(IDataAccessLayer dataAccessLayer)
        {
            _dataAccessLayer = dataAccessLayer;
        }

        public Task<MutationResult> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<FaqCategory>> Get()
        {
            DataTable table = await _dataAccessLayer.GetFaqCategories(new GetFaqCategoriesParameters());
            List<FaqCategory> faqcategories = new();

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

        public Task<FaqCategory> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FaqCategory>> GetByMerchant(Guid merchantId)
        {
            throw new NotImplementedException();
        }

        public Task<MutationResult> Insert(FaqCategory item)
        {
            throw new NotImplementedException();
        }

        public Task<MutationResult> Update(FaqCategory item)
        {
            throw new NotImplementedException();
        }
    }
}
