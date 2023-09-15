using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Data.Repositories
{
    public class ProductPhotoRepository : IProductPhotoRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer;

        public ProductPhotoRepository(IDataAccessLayer dataAccessLayer)
        {
            _dataAccessLayer = dataAccessLayer;
        }

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

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => _dataAccessLayer.DeleteProductPhoto(id, mutationId);

        private async Task<IEnumerable<ProductPhoto>> GetRaw(GetProductPhotosParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetProductPhotos(parameters);
            List<ProductPhoto> shops = new();

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