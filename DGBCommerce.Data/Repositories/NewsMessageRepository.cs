using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Data.Repositories
{
    public class NewsMessageRepository : INewsMessageRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer;

        public NewsMessageRepository(IDataAccessLayer dataAccessLayer)
        {
            _dataAccessLayer = dataAccessLayer;
        }

        public Task<MutationResult> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<NewsMessage>> Get()
        {
            DataTable table = await _dataAccessLayer.GetNewsMessages(new GetNewsMessagesParameters());
            List<NewsMessage> newsMessages = new();

            foreach (DataRow row in table.Rows)
            {
                newsMessages.Add(new NewsMessage()
                {
                    Id = new Guid(row["news_id"].ToString()!),
                    Title = Utilities.DbNullableString(row["nws_name"]),
                    Date = Convert.ToDateTime(row["nws_date"]),
                    Intro = Utilities.DbNullableString(row["nws_intro"]),
                });
            }

            return newsMessages;
        }

        public Task<NewsMessage> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<NewsMessage>> GetByMerchant(Guid merchantId)
        {
            throw new NotImplementedException();
        }

        public Task<MutationResult> Insert(NewsMessage item)
        {
            throw new NotImplementedException();
        }

        public Task<MutationResult> Update(NewsMessage item)
        {
            throw new NotImplementedException();
        }
    }
}
