﻿using YourCryptoShop.Domain;
using YourCryptoShop.Domain.Interfaces;
using YourCryptoShop.Domain.Interfaces.Repositories;
using YourCryptoShop.Domain.Models;
using YourCryptoShop.Domain.Models.ViewModels;
using YourCryptoShop.Domain.Parameters;
using System.Data;

namespace YourCryptoShop.Data.Repositories
{
    public class CategoryRepository(IDataAccessLayer dataAccessLayer) : ICategoryRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<Category>> Get(GetCategoriesParameters parameters)
            => await this.GetRaw(parameters);

        public async Task<Category?> GetById(Guid merchantId, Guid id)
        {
            var categories = await this.GetRaw(new GetCategoriesParameters() { MerchantId = merchantId, Id = id });
            return categories.ToList().SingleOrDefault();
        }

        public async Task<IEnumerable<PublicCategory>> GetByShopIdPublic(Guid shopId)
        {
            var categories = await GetRawPublic(new GetCategoriesParameters() { ShopId = shopId });
            return categories.ToList();
        }

        public Task<MutationResult> Create(Category item, Guid mutationId)
            => _dataAccessLayer.CreateCategory(item, mutationId);

        public Task<MutationResult> Update(Category item, Guid mutationId)
            => _dataAccessLayer.UpdateCategory(item, mutationId);

        public Task<MutationResult> MoveUp(Guid id, Guid? parentId, Guid mutationId)
            => _dataAccessLayer.UpdateCategoryMoveUp(id, parentId, mutationId);

        public Task<MutationResult> MoveDown(Guid id, Guid? parentId, Guid mutationId)
            => _dataAccessLayer.UpdateCategoryMoveDown(id, parentId, mutationId);

        public Task<MutationResult> ChangeParent(Guid id, Guid parentId, Guid mutationId)
            => _dataAccessLayer.UpdateCategoryChangeParent(id, parentId, mutationId);

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => _dataAccessLayer.DeleteCategory(id, mutationId);

        private async Task<IEnumerable<Category>> GetRaw(GetCategoriesParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetCategories(parameters);
            List<Category> categories = [];

            foreach (DataRow row in table.Rows)
            {
                categories.Add(new()
                {
                    Id = new Guid(row["cat_id"].ToString()!),
                    ShopId = new Guid(row["cat_shop"].ToString()!),
                    ParentId = Utilities.DbNullableGuid(row["cat_parent"]),
                    Name = Utilities.DbNullableString(row["cat_name"]),
                    Visible = Convert.ToBoolean(row["cat_visible"]),
                    SortOrder = Utilities.DbNullableInt(row["cat_sortorder"])
                });
            }

            return categories;
        }

        private async Task<IEnumerable<PublicCategory>> GetRawPublic(GetCategoriesParameters parameters)
        {
            // Only get visible categories
            parameters.Visible = true;

            DataTable table = await _dataAccessLayer.GetCategories(parameters);
            List<PublicCategory> categories = [];

            foreach (DataRow row in table.Rows)
            {
                categories.Add(new()
                {
                    Id = new Guid(row["cat_id"].ToString()!),
                    ParentId = Utilities.DbNullableGuid(row["cat_parent"]),
                    Name = Utilities.DbNullableString(row["cat_name"])
                });
            }

            return categories;
        }
    }
}