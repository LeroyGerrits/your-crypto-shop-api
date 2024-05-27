﻿using YourCryptoShop.Domain;
using YourCryptoShop.Domain.Interfaces;
using YourCryptoShop.Domain.Interfaces.Repositories;
using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Parameters;
using System.Data;

namespace YourCryptoShop.Data.Repositories
{
    public class FaqRepository(IDataAccessLayer dataAccessLayer) : IFaqRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<Faq>> Get(GetFaqsParameters parameters)
            => await GetRaw(parameters);

        public async Task<Faq?> GetById(Guid id)
        {
            var faqs = await GetRaw(new GetFaqsParameters() { Id = id });
            return faqs.ToList().SingleOrDefault();
        }

        private async Task<IEnumerable<Faq>> GetRaw(GetFaqsParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetFaqs(parameters);
            List<Faq> faqs = [];

            foreach (DataRow row in table.Rows)
            {
                Faq faq = new()
                {
                    Id = new Guid(row["faq_id"].ToString()!),
                    Category = new FaqCategory()
                    {
                        Id = new Guid(row["faq_category"].ToString()!),
                        Name = Utilities.DbNullableString(row["faq_category_name"])
                    },
                    Title = Utilities.DbNullableString(row["faq_title"]),
                    Content = Utilities.DbNullableString(row["faq_content"]),
                    SortOrder = Utilities.DbNullableInt(row["faq_sortorder"]),
                };

                if (row["faq_keywords"] != DBNull.Value)
                {
                    string keywords = Utilities.DbNullableString(row["faq_keywords"]);
                    faq.Keywords = keywords.Split(',');
                }

                faqs.Add(faq);
            }

            return faqs;
        }
    }
}