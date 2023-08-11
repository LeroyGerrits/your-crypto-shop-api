using DGBCommerce.Domain;
using DGBCommerce.Domain.Enums;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using System.Data;
using System.Linq;

namespace DGBCommerce.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer;

        public CategoryRepository(IDataAccessLayer dataAccessLayer)
        {
            _dataAccessLayer = dataAccessLayer;
        }

        public Task<MutationResult> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Category>> Get()
            => throw new InvalidOperationException($"A complete list of '{nameof(Category)}' objects may not be retrieved.");

        public async Task<IEnumerable<Category>> GetByMerchantId(Guid merchantId)
            => await this.GetRaw(new GetCategoriesParameters { MerchantId = merchantId });

        public async Task<IEnumerable<Category>> GetByParentId(Guid parentId)
            => await this.GetRaw(new GetCategoriesParameters { ParentId = parentId });

        public async Task<IEnumerable<Category>> GetByShopId(Guid shopId)
            => await this.GetRaw(new GetCategoriesParameters { ShopId = shopId });

        public async Task<Category?> GetById(Guid id)
        {
            var categories = await this.GetRaw(new GetCategoriesParameters() { Id = id });
            return categories.ToList().SingleOrDefault();
        }

        public Task<IEnumerable<Category>> GetByMerchant(Guid merchantId)
        {
            throw new NotImplementedException();
        }

        public Task<MutationResult> Create(Category item, Guid merchantId)
        {
            throw new NotImplementedException();
        }

        public Task<MutationResult> Update(Category item)
        {
            throw new NotImplementedException();
        }

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
                            LastName = Utilities.DbNullableString(row["cat_shop_merchant_lastname"]),
                        },
                        Name = Utilities.DbNullableString(row["cat_shop_name"]),
                    },
                    Name = Utilities.DbNullableString(row["cat_name"]),
                    SortOrder = Utilities.DbNullableInt(row["cat_sortorder"])
                };

                if (row["cat_parent"] != DBNull.Value)
                {
                    category.Parent = new Category()
                    {
                        Id = new Guid(row["cat_parent"].ToString()!),
                        Shop = category.Shop,
                        Name = Utilities.DbNullableString(row["cat_parent_name"])
                    };
                }

                categories.Add(category);
            }

            return categories;
        }
    }
}
