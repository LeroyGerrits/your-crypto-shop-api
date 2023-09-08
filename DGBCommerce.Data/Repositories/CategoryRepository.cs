using DGBCommerce.Domain;
using DGBCommerce.Domain.Enums;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer;

        public CategoryRepository(IDataAccessLayer dataAccessLayer)
        {
            _dataAccessLayer = dataAccessLayer;
        }

        public async Task<IEnumerable<Category>> Get(GetCategoriesParameters parameters)
            => await this.GetRaw(parameters);

        public async Task<Category?> GetById(Guid merchantId, Guid id)
        {
            var categories = await this.GetRaw(new GetCategoriesParameters() { MerchantId = merchantId, Id = id });
            return categories.ToList().SingleOrDefault();
        }

        public Task<MutationResult> Create(Category item, Guid mutationId)
            => _dataAccessLayer.CreateCategory(item, mutationId);

        public Task<MutationResult> Update(Category item, Guid mutationId)
            => _dataAccessLayer.UpdateCategory(item, mutationId);

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => _dataAccessLayer.DeleteCategory(id, mutationId);

        private async Task<IEnumerable<Category>> GetRaw(GetCategoriesParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetCategories(parameters);
            List<Category> categories = new();

            foreach (DataRow row in table.Rows)
            {
                Category category = new()
                {
                    Id = new Guid(row["cat_id"].ToString()!),
                    Shop = new Shop()
                    {
                        Id = new Guid(row["cat_shop"].ToString()!),
                        Merchant = new Merchant()
                        {
                            Id = new Guid(row["cat_shop_merchant"].ToString()!),
                            EmailAddress = Utilities.DbNullableString(row["cat_shop_merchant_email_address"]),
                            Gender = (Gender)Convert.ToInt32(row["cat_shop_merchant_gender"]),
                            FirstName = Utilities.DbNullableString(row["cat_shop_merchant_first_name"]),
                            LastName = Utilities.DbNullableString(row["cat_shop_merchant_last_name"]),
                        },
                        Name = Utilities.DbNullableString(row["cat_shop_name"]),
                    },
                    Name = Utilities.DbNullableString(row["cat_name"]),
                    Visible = Convert.ToBoolean(row["cat_visible"]),
                    SortOrder = Utilities.DbNullableInt(row["cat_sortorder"])
                };

                if (row["cat_parent"] != DBNull.Value)
                {
                    category.Parent = new Category()
                    {
                        Id = new Guid(row["cat_parent"].ToString()!),
                        Shop = category.Shop,
                        Name = Utilities.DbNullableString(row["cat_parent_name"]),
                        Visible = Convert.ToBoolean(row["cat_parent_visible"])
                    };
                }

                categories.Add(category);
            }

            return categories;
        }
    }
}
