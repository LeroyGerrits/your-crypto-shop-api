using YourCryptoShop.Domain;
using YourCryptoShop.Domain.Interfaces;
using YourCryptoShop.Domain.Interfaces.Repositories;
using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;
using System.Data;

namespace YourCryptoShop.Data.Repositories
{
    public class NewsMessageRepository(IDataAccessLayer dataAccessLayer) : INewsMessageRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<NewsMessage>> Get(GetNewsMessagesParameters parameters)
            => await GetRaw(parameters);

        public async Task<NewsMessage?> GetById(Guid id)
        {
            var newsMessages = await GetRaw(new GetNewsMessagesParameters() { Id = id });
            return newsMessages.ToList().SingleOrDefault();
        }

        private async Task<IEnumerable<NewsMessage>> GetRaw(GetNewsMessagesParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetNewsMessages(parameters);
            List<NewsMessage> newsMessages = [];

            foreach (DataRow row in table.Rows)
            {
                newsMessages.Add(new NewsMessage()
                {
                    Id = new Guid(row["nws_id"].ToString()!),
                    Date = Convert.ToDateTime(row["nws_date"]),
                    ThumbnailUrl = Utilities.DbNullableString(row["nws_thumbnail_url"]),
                    Title = Utilities.DbNullableString(row["nws_title"]),
                    Intro = Utilities.DbNullableString(row["nws_intro"]),
                    Content = Utilities.DbNullableString(row["nws_content"])
                });
            }

            return newsMessages;
        }
    }
}