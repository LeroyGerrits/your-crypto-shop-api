using DGBCommerce.Domain;
using DGBCommerce.Domain.Interfaces;
using DGBCommerce.Domain.Interfaces.Repositories;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Parameters;
using System.Data;

namespace DGBCommerce.Data.Repositories
{
    public class ProductRepository(IDataAccessLayer dataAccessLayer) : IProductRepository
    {
        private readonly IDataAccessLayer _dataAccessLayer = dataAccessLayer;

        public async Task<IEnumerable<Product>> Get(GetProductsParameters parameters)
            => await GetRaw(parameters);

        public async Task<Product?> GetById(Guid merchantId, Guid id)
        {
            var shops = await GetRaw(new GetProductsParameters() { MerchantId = merchantId, Id = id });
            return shops.ToList().SingleOrDefault();
        }

        public Task<MutationResult> Create(Product item, Guid mutationId)
            => _dataAccessLayer.CreateProduct(item, mutationId);

        public Task<MutationResult> Update(Product item, Guid mutationId)
            => _dataAccessLayer.UpdateProduct(item, mutationId);

        public Task<MutationResult> Delete(Guid id, Guid mutationId)
            => _dataAccessLayer.DeleteProduct(id, mutationId);

        private async Task<IEnumerable<Product>> GetRaw(GetProductsParameters parameters)
        {
            DataTable table = await _dataAccessLayer.GetProducts(parameters);
            List<Product> shops = [];

            foreach (DataRow row in table.Rows)
            {
                shops.Add(new Product()
                {
                    Id = new Guid(row["prd_id"].ToString()!),
                    ShopId = new Guid(row["prd_shop"].ToString()!),
                    Name = Utilities.DbNullableString(row["prd_name"]),
                    Description = Utilities.DbNullableString(row["prd_description"]),
                    Stock = Utilities.DbNullableInt(row["prd_stock"]),
                    Price = Convert.ToDecimal(row["prd_price"]),
                    Visible = Convert.ToBoolean(row["prd_visible"]),
                    ShowOnHome = Convert.ToBoolean(row["prd_show_on_home"]),
                    MainPhotoId = Utilities.DbNullableGuid(row["prd_main_photo_id"]),
                    MainPhotoExtension = Utilities.DbNullableString(row["prd_main_photo_extension"])
                });
            }

            return shops;
        }
    }
}