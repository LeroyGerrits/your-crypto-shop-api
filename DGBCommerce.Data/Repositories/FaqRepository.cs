using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Data.Repositories
{
    public class FaqRepository : IFaqRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer;

        public FaqRepository(IDataAccessLayer dataAccessLayer)
        {
            _dataAccessLayer = dataAccessLayer;
        }

        public Task<MutationResult> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Faq>> Get()
        {
            DataTable table = await _dataAccessLayer.GetFaqs(new GetFaqsParameters());
            List<Faq> newsMessages = new();

            foreach (DataRow row in table.Rows)
            {
                newsMessages.Add(new Faq()
                {
                    Id = new Guid(row["faq_id"].ToString()!),
                    Category = new FaqCategory()
                    {
                        Id = new Guid(row["faq_category"].ToString()!),
                        Name = Utilities.DbNullableString(row["faq_category_name"])
                    },
                    Title = Utilities.DbNullableString(row["faq_title"]),
                    Keywords = Utilities.DbNullableString(row["faq_keywords"]),
                    Content = Utilities.DbNullableString(row["faq_content"]),
                    SortOrder = Utilities.DbNullableInt(row["faq_sortorder"]),
                });
            }

            return newsMessages;
        }

        public Task<Faq> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Faq>> GetByMerchant(Guid merchantId)
        {
            throw new NotImplementedException();
        }

        public Task<MutationResult> Insert(Faq item)
        {
            throw new NotImplementedException();
        }

        public Task<MutationResult> Update(Faq item)
        {
            throw new NotImplementedException();
        }
    }
}
