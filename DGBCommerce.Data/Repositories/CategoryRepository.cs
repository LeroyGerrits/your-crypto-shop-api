using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Interfaces.Repositories;
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
            List<Category> categories = new();

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
    }
}