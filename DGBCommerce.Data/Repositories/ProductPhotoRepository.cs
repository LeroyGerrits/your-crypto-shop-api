using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Data.Repositories
{
    public class ProductPhotoRepository(IDataAccessLayer dataAccessLayer) : IProductPhotoRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<ProductPhoto>> Get(GetProductPhotosParameters parameters)
            => await GetRaw(parameters);

        public async Task<ProductPhoto?> GetById(Guid merchantId, Guid id)
        {
            var shops = await GetRaw(new GetProductPhotosParameters() { MerchantId = merchantId, Id = id });
            return shops.ToList().SingleOrDefault();
        }

        public Task<MutationResult> Create(ProductPhoto item, Guid mutationId)
            => _dataAccessLayer.CreateProductPhoto(item, mutationId);

        public Task<MutationResult> Update(ProductPhoto item, Guid mutationId)
            => _dataAccessLayer.UpdateProductPhoto(item, mutationId);

        public Task<MutationResult> ChangeDescription(Guid id, string description, Guid mutationId)
            => _dataAccessLayer.UpdateProductPhotoChangeDescription(id, description, mutationId);

        public Task<MutationResult> ChangeMain(Guid id, Guid productId, Guid mutationId)
            => _dataAccessLayer.UpdateProductPhotoChangeMain(id, productId, mutationId);

        public Task<MutationResult> ChangeVisible(Guid id, bool visible, Guid mutationId)
            => _dataAccessLayer.UpdateProductPhotoChangeVisible(id, visible, mutationId);

        public Task<MutationResult> MoveUp(Guid id, Guid productId, Guid mutationId)
            => _dataAccessLayer.UpdateProductPhotoMoveUp(id, productId, mutationId);

        public Task<MutationResult> MoveDown(Guid id, Guid productId, Guid mutationId)
            => _dataAccessLayer.UpdateProductPhotoMoveDown(id, productId, mutationId);

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => _dataAccessLayer.DeleteProductPhoto(id, mutationId);

        private async Task<IEnumerable<ProductPhoto>> GetRaw(GetProductPhotosParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetProductPhotos(parameters);
            List<ProductPhoto> shops = [];

            foreach (DataRow row in table.Rows)
            {
                shops.Add(new ProductPhoto()
                {
                    Id = new Guid(row["pht_id"].ToString()!),
                    ProductId = new Guid(row["pht_product"].ToString()!),
                    File = Utilities.DbNullableString(row["pht_file"]),
                    Extension = Utilities.DbNullableString(row["pht_extension"]),
                    FileSize = Convert.ToInt32(row["pht_file_size"]),
                    Width = Convert.ToInt32(row["pht_width"]),
                    Height = Convert.ToInt32(row["pht_height"]),
                    Description = Utilities.DbNullableString(row["pht_description"]),
                    SortOrder = Utilities.DbNullableInt(row["pht_sortorder"]),
                    Main = Convert.ToBoolean(row["pht_main"]),
                    Visible = Convert.ToBoolean(row["pht_visible"])
                });
            }

            return shops;
        }
    }
}