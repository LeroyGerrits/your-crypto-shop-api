using System.Data;
using Microsoft.Data.SqlClient;
using DGBCommerce.Domain.Parameters;
using DGBCommerce.Domain;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Enums;
using DGBCommerce.Domain.Interfaces;

namespace DGBCommerce.Data
{
    public class DataAccessLayer : IDataAccessLayer
    {
        private readonly string _connectionString;

        public DataAccessLayer(string connectionString)
        {
            _connectionString = connectionString;
        }

        #region Create
        public async Task<MutationResult> CreateCategory(Category category, Guid mutationId)
            => await NonQuery("SP_MUTATE_Category", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Create },
                new SqlParameter("@CAT_SHOP", SqlDbType.UniqueIdentifier) { Value = category.ShopId },
                new SqlParameter("@CAT_PARENT", SqlDbType.UniqueIdentifier) { Value = category.ParentId },
                new SqlParameter("@CAT_NAME", SqlDbType.NVarChar, 255) { Value = category.Name },
                new SqlParameter("@CAT_VISIBLE", SqlDbType.Bit) { Value = category.Visible }
            }, mutationId);

        public async Task<MutationResult> CreateDeliveryMethod(DeliveryMethod deliveryMethod, Guid mutationId)
            => await NonQuery("SP_MUTATE_DeliveryMethod", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Create },
                new SqlParameter("@DLM_SHOP", SqlDbType.UniqueIdentifier) { Value = deliveryMethod.Shop.Id },
                new SqlParameter("@DLM_NAME", SqlDbType.NVarChar, 255) { Value = deliveryMethod.Name },
                new SqlParameter("@DLM_COSTS", SqlDbType.Decimal) { Value = deliveryMethod.Costs }
            }, mutationId);

        public async Task<MutationResult> CreateDigiByteWallet(DigiByteWallet digiByteWallet, Guid mutationId)
            => await NonQuery("SP_MUTATE_DigiByteWallet", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Create },
                new SqlParameter("@DBW_MERCHANT", SqlDbType.UniqueIdentifier) { Value = digiByteWallet.Merchant.Id },
                new SqlParameter("@DBW_NAME", SqlDbType.NVarChar, 255) { Value = digiByteWallet.Name },
                new SqlParameter("@DBW_ADDRESS", SqlDbType.VarChar, 100) { Value = digiByteWallet.Address }
            }, mutationId);

        public async Task<MutationResult> CreateMerchant(Merchant merchant, Guid mutationId)
            => await NonQuery("SP_MUTATE_Merchant", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Create },
                new SqlParameter("@MER_EMAIL_ADDRESS", SqlDbType.VarChar) { Value = merchant.EmailAddress },
                new SqlParameter("@MER_USERNAME", SqlDbType.VarChar) { Value = merchant.Username },
                new SqlParameter("@MER_PASSWORD_SALT", SqlDbType.VarChar) { Value = merchant.PasswordSalt },
                new SqlParameter("@MER_PASSWORD", SqlDbType.VarChar) { Value = merchant.Password },
                new SqlParameter("@MER_GENDER", SqlDbType.TinyInt) { Value = merchant.Gender },
                new SqlParameter("@MER_FIRST_NAME", SqlDbType.NVarChar) { Value = merchant.FirstName },
                new SqlParameter("@MER_LAST_NAME", SqlDbType.NVarChar) { Value = merchant.LastName }
            }, mutationId);

        public async Task<MutationResult> CreateMerchantPasswordResetLink(MerchantPasswordResetLink merchantPasswordResetLink)
            => await NonQuery("SP_MUTATE_MerchantPasswordResetLink", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Create },
                new SqlParameter("@PRL_MERCHANT", SqlDbType.UniqueIdentifier) { Value = merchantPasswordResetLink.Merchant.Id },
                new SqlParameter("@PRL_IP_ADDRESS", SqlDbType.VarChar) { Value = merchantPasswordResetLink.IpAddress },
                new SqlParameter("@PRL_KEY", SqlDbType.VarChar) { Value = merchantPasswordResetLink.Key }
            });

        public async Task<MutationResult> CreateProduct(Product product, Guid mutationId)
            => await NonQuery("SP_MUTATE_Product", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Create },
                new SqlParameter("@PRD_SHOP", SqlDbType.UniqueIdentifier) { Value = product.ShopId },
                new SqlParameter("@PRD_NAME", SqlDbType.NVarChar) { Value = product.Name },
                new SqlParameter("@PRD_DESCRIPTION", SqlDbType.NVarChar) { Value = product.Description },
                new SqlParameter("@PRD_STOCK", SqlDbType.Int) { Value = product.Stock },
                new SqlParameter("@PRD_PRICE", SqlDbType.Decimal) { Value = product.Price },
                new SqlParameter("@PRD_VISIBLE", SqlDbType.NVarChar) { Value = product.Visible }
            }, mutationId);

        public async Task<MutationResult> CreateProductCategory(ProductCategory productCategory, Guid mutationId)
            => await NonQuery("SP_MUTATE_ProductCategory", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Create },
                new SqlParameter("@P2C_PRODUCT", SqlDbType.UniqueIdentifier) { Value = productCategory.ProductId },
                new SqlParameter("@P2C_CATEGORY", SqlDbType.UniqueIdentifier) { Value = productCategory.CategoryId }
            }, mutationId);

        public async Task<MutationResult> CreateProductPhoto(ProductPhoto productPhoto, Guid mutationId)
            => await NonQuery("SP_MUTATE_ProductPhoto", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Create },
                new SqlParameter("@PHT_PRODUCT", SqlDbType.UniqueIdentifier) { Value = productPhoto.ProductId },
                new SqlParameter("@PHT_FILE", SqlDbType.VarChar) { Value = productPhoto.File },
                new SqlParameter("@PHT_EXTENSION", SqlDbType.Char) { Value = productPhoto.Extension },
                new SqlParameter("@PHT_FILE_SIZE", SqlDbType.Int) { Value = productPhoto.FileSize },
                new SqlParameter("@PHT_WIDTH", SqlDbType.Int) { Value = productPhoto.Width },
                new SqlParameter("@PHT_HEIGHT", SqlDbType.Int) { Value = productPhoto.Height },
                new SqlParameter("@PRD_DESCRIPTION", SqlDbType.NVarChar) { Value = productPhoto.Description },
                new SqlParameter("@PHT_SORTORDER", SqlDbType.Int) { Value = productPhoto.SortOrder },
                new SqlParameter("@PHT_MAIN", SqlDbType.Bit) { Value = productPhoto.Main },
                new SqlParameter("@PHT_VISIBLE", SqlDbType.Bit) { Value = productPhoto.Visible }
            }, mutationId);

        public async Task<MutationResult> CreateShop(Shop shop, Guid mutationId)
            => await NonQuery("SP_MUTATE_Shop", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Create },
                new SqlParameter("@SHP_MERCHANT", SqlDbType.UniqueIdentifier) { Value = shop.MerchantId },
                new SqlParameter("@SHP_NAME", SqlDbType.NVarChar) { Value = shop.Name },
                new SqlParameter("@SHP_SUBDOMAIN", SqlDbType.VarChar) { Value = shop.SubDomain }
            }, mutationId);
        #endregion

        #region Delete
        public async Task<MutationResult> DeleteCategory(Guid categoryId, Guid mutationId)
            => await NonQuery("SP_MUTATE_Category", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Delete },
                new SqlParameter("@CAT_ID", SqlDbType.UniqueIdentifier) { Value = categoryId }
            }, mutationId);

        public async Task<MutationResult> DeleteDeliveryMethod(Guid deliveryMethodId, Guid mutationId)
            => await NonQuery("SP_MUTATE_DeliveryMethod", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Delete },
                new SqlParameter("@DLM_ID", SqlDbType.UniqueIdentifier) { Value = deliveryMethodId }
            }, mutationId);

        public async Task<MutationResult> DeleteDigiByteWallet(Guid digiByteWalletId, Guid mutationId)
            => await NonQuery("SP_MUTATE_DigiByteWallet", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Delete },
                new SqlParameter("@DBW_ID", SqlDbType.UniqueIdentifier) { Value = digiByteWalletId }
            }, mutationId);

        public async Task<MutationResult> DeleteProduct(Guid shopId, Guid mutationId)
            => await NonQuery("SP_MUTATE_Product", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Delete },
                new SqlParameter("@PRD_ID", SqlDbType.UniqueIdentifier) { Value = shopId }
            }, mutationId);

        public async Task<MutationResult> DeleteProductCategory(Guid productId, Guid categoryId, Guid mutationId)
            => await NonQuery("SP_MUTATE_ProductCategory", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Delete },
                new SqlParameter("@P2C_PRODUCT", SqlDbType.UniqueIdentifier) { Value = productId },
                new SqlParameter("@P2C_CATEGORY", SqlDbType.UniqueIdentifier) { Value = categoryId }
            }, mutationId);

        public async Task<MutationResult> DeleteProductPhoto(Guid productPhotoId, Guid mutationId)
            => await NonQuery("SP_MUTATE_ProductPhoto", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Delete },
                new SqlParameter("@PHT_ID", SqlDbType.UniqueIdentifier) { Value = productPhotoId }
            }, mutationId);

        public async Task<MutationResult> DeleteShop(Guid shopId, Guid mutationId)
            => await NonQuery("SP_MUTATE_Shop", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Delete },
                new SqlParameter("@SHP_ID", SqlDbType.UniqueIdentifier) { Value = shopId }
            }, mutationId);
        #endregion

        #region Get
        public async Task<DataTable> GetCategories(GetCategoriesParameters parameters)
            => await Get("SP_GET_Categories", new List<SqlParameter>() {
                new SqlParameter("@CAT_ID", SqlDbType.UniqueIdentifier){ Value = parameters.Id },
                new SqlParameter("@CAT_SHOP", SqlDbType.UniqueIdentifier) { Value = parameters.ShopId },
                new SqlParameter("@CAT_SHOP_MERCHANT", SqlDbType.UniqueIdentifier) { Value = parameters.MerchantId },
                new SqlParameter("@CAT_PARENT", SqlDbType.UniqueIdentifier) { Value = parameters.ParentId },
                new SqlParameter("@CAT_NAME", SqlDbType.NVarChar) { Value = parameters.Name }
            });

        public async Task<DataTable> GetCountries(GetCountriesParameters parameters)
            => await Get("SP_GET_Countries", new List<SqlParameter>() {
                new SqlParameter("@CTR_ID", SqlDbType.UniqueIdentifier){ Value = parameters.Id },
                new SqlParameter("@CTR_CODE", SqlDbType.Char) { Value = parameters.Code },
                new SqlParameter("@CTR_NAME", SqlDbType.VarChar) { Value = parameters.Name }
            });

        public async Task<DataTable> GetCurrencies(GetCurrenciesParameters parameters)
            => await Get("SP_GET_Currencies", new List<SqlParameter>() {
                new SqlParameter("@CUR_ID", SqlDbType.UniqueIdentifier){ Value = parameters.Id },
                new SqlParameter("@CUR_SYMBOL", SqlDbType.NChar) { Value = parameters.Symbol },
                new SqlParameter("@CUR_NAME", SqlDbType.VarChar) { Value = parameters.Name }
            });

        public async Task<DataTable> GetDeliveryMethods(GetDeliveryMethodsParameters parameters)
            => await Get("SP_GET_DeliveryMethods", new List<SqlParameter>() {
                new SqlParameter("@DLM_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@DLM_SHOP", SqlDbType.UniqueIdentifier) { Value = parameters.ShopId },
                new SqlParameter("@DLM_NAME", SqlDbType.NVarChar) { Value = parameters.Name }
            });

        public async Task<DataTable> GetDigiByteWallets(GetDigiByteWalletsParameters parameters)
            => await Get("SP_GET_DigiByteWallets", new List<SqlParameter>() {
                new SqlParameter("@DBW_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@DBW_MERCHANT", SqlDbType.UniqueIdentifier) { Value = parameters.MerchantId },
                new SqlParameter("@DBW_NAME", SqlDbType.NVarChar) { Value = parameters.Name },
                new SqlParameter("@DBW_ADDRESS", SqlDbType.VarChar) { Value = parameters.Address }
            });

        public async Task<DataTable> GetFaqCategories(GetFaqCategoriesParameters parameters)
            => await Get("SP_GET_FaqCategories", new List<SqlParameter>() {
                new SqlParameter("@CAT_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@CAT_NAME", SqlDbType.NVarChar) { Value = parameters.Name }
            });

        public async Task<DataTable> GetFaqs(GetFaqsParameters parameters)
            => await Get("SP_GET_Faqs", new List<SqlParameter>() {
                new SqlParameter("@FAQ_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@FAQ_CATEGORY", SqlDbType.UniqueIdentifier) { Value = parameters.CategoryId },
                new SqlParameter("@FAQ_TITLE", SqlDbType.NVarChar) { Value = parameters.Title },
                new SqlParameter("@FAQ_KEYWORDS", SqlDbType.NVarChar) { Value = parameters.Keywords },
            });

        public async Task<DataTable> GetFinancialStatementTransactions(GetFinancialStatementTransactionsParameters parameters)
            => await Get("SP_GET_FinancialStatementTransactions", new List<SqlParameter>() {
                new SqlParameter("@TRX_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@TRX_DATE_FROM", SqlDbType.Date) { Value = parameters.DateFrom },
                new SqlParameter("@TRX_DATE_UNTIL", SqlDbType.Date) { Value = parameters.DateUntil },
                new SqlParameter("@TRX_TYPE", SqlDbType.TinyInt) { Value = parameters.Type },
                new SqlParameter("@TRX_CURRENCY", SqlDbType.UniqueIdentifier) { Value = parameters.CurrencyId },
                new SqlParameter("@TRX_RECURRANCE", SqlDbType.TinyInt) { Value = parameters.Recurrance },
                new SqlParameter("@TRX_DESCRIPTION", SqlDbType.NVarChar) { Value = parameters.Description },
            });

        public async Task<DataTable> GetNewsMessages(GetNewsMessagesParameters parameters)
            => await Get("SP_GET_NewsMessages", new List<SqlParameter>() {
                new SqlParameter("@NWS_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@NWS_TITLE", SqlDbType.NVarChar) { Value = parameters.Title },
                new SqlParameter("@NWS_DATE_FROM", SqlDbType.DateTime) { Value = parameters.DateFrom },
                new SqlParameter("@NWS_DATE_UNTIL", SqlDbType.DateTime) { Value = parameters.DateUntil }
            });

        public async Task<DataTable> GetMerchantByEmailAddress(string emailAddress)
            => await Get("SP_GET_Merchant_ByEmailAddress", new List<SqlParameter>() {
                new SqlParameter("@MER_EMAIL_ADDRESS", SqlDbType.VarChar) { Value = emailAddress }
            });

        public async Task<DataTable> GetMerchantByEmailAddressAndPassword(string emailAddress, string password, string? ipAddress)
            => await Get("SP_GET_Merchant_ByEmailAddressAndPassword", new List<SqlParameter>() {
                new SqlParameter("@MER_EMAIL_ADDRESS", SqlDbType.VarChar) { Value = emailAddress },
                new SqlParameter("@MER_PASSWORD", SqlDbType.VarChar) { Value = password },
                new SqlParameter("@MER_IP_ADDRESS", SqlDbType.VarChar) { Value = ipAddress }
            });

        public async Task<DataTable> GetMerchantByIdAndPassword(Guid id, string password)
            => await Get("SP_GET_Merchant_ByIdAndPassword", new List<SqlParameter>() {
                new SqlParameter("@MER_ID", SqlDbType.UniqueIdentifier) { Value = id },
                new SqlParameter("@MER_PASSWORD", SqlDbType.VarChar) { Value = password }
            });

        public async Task<DataTable> GetMerchantPasswordResetLinkByIdAndKey(Guid id, string key)
            => await Get("SP_GET_Merchant_ByEmailAddressAndPassword", new List<SqlParameter>() {
                new SqlParameter("@PRL_ID", SqlDbType.VarChar) { Value = id },
                new SqlParameter("@PRL_KEY", SqlDbType.VarChar) { Value = key },
            });

        public async Task<DataTable> GetMerchants(GetMerchantsParameters parameters)
            => await Get("SP_GET_Merchants", new List<SqlParameter>() {
                new SqlParameter("@MER_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@MER_EMAIL_ADDRESS", SqlDbType.VarChar) { Value = parameters.EmailAddress },
                new SqlParameter("@MER_USERNAME", SqlDbType.VarChar) { Value = parameters.Username },
                new SqlParameter("@MER_PASSWORD", SqlDbType.VarChar) { Value = parameters.Password },
                new SqlParameter("@MER_FIRST_NAME", SqlDbType.NVarChar) { Value = parameters.FirstName },
                new SqlParameter("@MER_LAST_NAME", SqlDbType.NVarChar) { Value = parameters.LastName }
            });

        public async Task<DataTable> GetProducts(GetProductsParameters parameters)
            => await Get("SP_GET_Products", new List<SqlParameter>() {
                new SqlParameter("@PRD_ID", SqlDbType.UniqueIdentifier){ Value = parameters.Id },
                new SqlParameter("@PRD_SHOP_MERCHANT_ID", SqlDbType.UniqueIdentifier){ Value = parameters.MerchantId },
                new SqlParameter("@PRD_SHOP", SqlDbType.UniqueIdentifier) { Value = parameters.ShopId },
                new SqlParameter("@PRD_CATEGORY", SqlDbType.UniqueIdentifier) { Value = parameters.CategoryId },
                new SqlParameter("@PRD_NAME", SqlDbType.NVarChar) { Value = parameters.Name }
            });

        public async Task<DataTable> GetProductCategories(GetProductCategoriesParameters parameters)
            => await Get("SP_GET_ProductCategories", new List<SqlParameter>() {
                new SqlParameter("@P2C_MERCHANT", SqlDbType.UniqueIdentifier){ Value = parameters.MerchantId },
                new SqlParameter("@P2C_PRODUCT", SqlDbType.UniqueIdentifier){ Value = parameters.ProductId },
                new SqlParameter("@P2C_CATEGORY", SqlDbType.UniqueIdentifier){ Value = parameters.CategoryId }
            });

        public async Task<DataTable> GetProductPhotos(GetProductPhotosParameters parameters)
            => await Get("SP_GET_ProductPhotos", new List<SqlParameter>() {
                new SqlParameter("@PHT_ID", SqlDbType.UniqueIdentifier){ Value = parameters.Id },
                new SqlParameter("@PHT_PRODUCT_SHOP_MERCHANT_ID", SqlDbType.UniqueIdentifier){ Value = parameters.MerchantId },
                new SqlParameter("@PHT_PRODUCT", SqlDbType.UniqueIdentifier) { Value = parameters.ProductId }
            });

        public async Task<DataTable> GetShops(GetShopsParameters parameters)
            => await Get("SP_GET_Shops", new List<SqlParameter>() {
                new SqlParameter("@SHP_ID", SqlDbType.UniqueIdentifier){ Value = parameters.Id },
                new SqlParameter("@SHP_MERCHANT", SqlDbType.UniqueIdentifier){ Value = parameters.MerchantId },
                new SqlParameter("@SHP_NAME", SqlDbType.NVarChar) { Value = parameters.Name },
                new SqlParameter("@SHP_SUBDOMAIN", SqlDbType.NVarChar) { Value = parameters.SubDomain },
                new SqlParameter("@SHP_FEATURED", SqlDbType.Bit) { Value = parameters.Featured }
            });

        public async Task<DataTable> GetShopBySubDomain(string subDomain)
            => await Get("SP_GET_Shop_BySubDomain", new List<SqlParameter>() {
                new SqlParameter("@SHP_SUBDOMAIN", SqlDbType.NVarChar) { Value = subDomain }
            });
        #endregion

        #region Update
        public async Task<MutationResult> UpdateCategory(Category category, Guid mutationId)
            => await NonQuery("SP_MUTATE_Category", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Update },
                new SqlParameter("@CAT_ID", SqlDbType.UniqueIdentifier) { Value = category.Id },
                new SqlParameter("@CAT_SHOP", SqlDbType.UniqueIdentifier) { Value = category.ShopId },
                new SqlParameter("@CAT_PARENT", SqlDbType.UniqueIdentifier) { Value = category.ParentId },
                new SqlParameter("@CAT_NAME", SqlDbType.NVarChar, 255) { Value = category.Name },
                new SqlParameter("@CAT_VISIBLE", SqlDbType.Bit) { Value = category.Visible }
            }, mutationId);

        public async Task<MutationResult> UpdateCategoryChangeParent(Guid categoryId, Guid parentId, Guid mutationId)
            => await NonQuery("SP_MUTATE_Category", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = 21 },
                new SqlParameter("@CAT_ID", SqlDbType.UniqueIdentifier) { Value = categoryId },
                new SqlParameter("@CAT_PARENT", SqlDbType.UniqueIdentifier) { Value = parentId }
            }, mutationId);

        public async Task<MutationResult> UpdateCategoryMoveDown(Guid categoryId, Guid? parentId, Guid mutationId)
            => await NonQuery("SP_MUTATE_Category", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = 22 },
                new SqlParameter("@CAT_ID", SqlDbType.UniqueIdentifier) { Value = categoryId },
                new SqlParameter("@CAT_PARENT", SqlDbType.UniqueIdentifier) { Value = parentId }
            }, mutationId);

        public async Task<MutationResult> UpdateCategoryMoveUp(Guid categoryId, Guid? parentId, Guid mutationId)
            => await NonQuery("SP_MUTATE_Category", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = 23 },
                new SqlParameter("@CAT_ID", SqlDbType.UniqueIdentifier) { Value = categoryId },
                new SqlParameter("@CAT_PARENT", SqlDbType.UniqueIdentifier) { Value = parentId }
            }, mutationId);

        public async Task<MutationResult> UpdateDeliveryMethod(DeliveryMethod deliveryMethod, Guid mutationId)
            => await NonQuery("SP_MUTATE_DeliveryMethod", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Update },
                new SqlParameter("@DLM_ID", SqlDbType.UniqueIdentifier) { Value = deliveryMethod.Id },
                new SqlParameter("@DLM_SHOP", SqlDbType.UniqueIdentifier) { Value = deliveryMethod.Shop.Id },
                new SqlParameter("@DLM_NAME", SqlDbType.NVarChar, 255) { Value = deliveryMethod.Name },
                new SqlParameter("@DLM_COSTS", SqlDbType.Decimal) { Value = deliveryMethod.Costs }
            }, mutationId);

        public async Task<MutationResult> UpdateDigiByteWallet(DigiByteWallet digiByteWallet, Guid mutationId)
            => await NonQuery("SP_MUTATE_DigiByteWallet", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Update },
                new SqlParameter("@DBW_ID", SqlDbType.UniqueIdentifier) { Value = digiByteWallet.Id },
                new SqlParameter("@DBW_MERCHANT", SqlDbType.UniqueIdentifier) { Value = digiByteWallet.Merchant.Id },
                new SqlParameter("@DBW_NAME", SqlDbType.NVarChar, 255) { Value = digiByteWallet.Name },
                new SqlParameter("@DBW_ADDRESS", SqlDbType.VarChar, 100) { Value = digiByteWallet.Address }
            }, mutationId);

        public async Task<MutationResult> UpdateMerchant(Merchant merchant, Guid mutationId)
            => await NonQuery("SP_MUTATE_Merchant", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Update },
                new SqlParameter("@MER_ID", SqlDbType.UniqueIdentifier) { Value = merchant.Id },
                new SqlParameter("@MER_FIRST_NAME", SqlDbType.NVarChar, 255) { Value = merchant.FirstName },
                new SqlParameter("@MER_LAST_NAME", SqlDbType.NVarChar, 255) { Value = merchant.LastName }
            }, mutationId);

        public async Task<MutationResult> UpdateMerchantPassword(Merchant merchant, string password, Guid mutationId)
            => await NonQuery("SP_MUTATE_Merchant", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = 21 },
                new SqlParameter("@MER_ID", SqlDbType.UniqueIdentifier) { Value = merchant.Id },
                new SqlParameter("@MER_PASSWORD", SqlDbType.VarChar, 100) { Value = password }
            }, mutationId);

        public async Task<MutationResult> UpdateProduct(Product product, Guid mutationId)
            => await NonQuery("SP_MUTATE_Product", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Update },
                new SqlParameter("@PRD_ID", SqlDbType.UniqueIdentifier) { Value = product.Id },
                new SqlParameter("@PRD_SHOP", SqlDbType.UniqueIdentifier) { Value = product.ShopId },
                new SqlParameter("@PRD_NAME", SqlDbType.NVarChar) { Value = product.Name },
                new SqlParameter("@PRD_DESCRIPTION", SqlDbType.NVarChar) { Value = product.Description },
                new SqlParameter("@PRD_STOCK", SqlDbType.Int) { Value = product.Stock },
                new SqlParameter("@PRD_PRICE", SqlDbType.Decimal) { Value = product.Price },
                new SqlParameter("@PRD_VISIBLE", SqlDbType.NVarChar) { Value = product.Visible }
            }, mutationId);

        public async Task<MutationResult> UpdateProductPhoto(ProductPhoto productPhoto, Guid mutationId)
            => await NonQuery("SP_MUTATE_ProductPhoto", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Update },
                new SqlParameter("@PHT_ID", SqlDbType.UniqueIdentifier) { Value = productPhoto.Id },
                new SqlParameter("@PRD_DESCRIPTION", SqlDbType.NVarChar) { Value = productPhoto.Description },
                new SqlParameter("@PHT_SORTORDER", SqlDbType.Int) { Value = productPhoto.SortOrder },
                new SqlParameter("@PHT_MAIN", SqlDbType.Bit) { Value = productPhoto.Main },
                new SqlParameter("@PHT_VISIBLE", SqlDbType.Bit) { Value = productPhoto.Visible }
            }, mutationId);

        public async Task<MutationResult> UpdateShop(Shop shop, Guid mutationId)
            => await NonQuery("SP_MUTATE_Shop", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Update },
                new SqlParameter("@SHP_ID", SqlDbType.UniqueIdentifier) { Value = shop.Id },
                new SqlParameter("@SHP_MERCHANT", SqlDbType.UniqueIdentifier) { Value = shop.MerchantId },
                new SqlParameter("@SHP_NAME", SqlDbType.NVarChar, 255) { Value = shop.Name },
                new SqlParameter("@SHP_SUBDOMAIN", SqlDbType.VarChar, 100) { Value = shop.SubDomain }
            }, mutationId);
        #endregion

        private async Task<DataTable> Get(string storedProcedure, List<SqlParameter> parameters)
        {
            DataTable table = new();

            using (SqlConnection connection = new(_connectionString))
            {
                using SqlCommand command = new(storedProcedure, connection) { CommandType = CommandType.StoredProcedure };
                foreach (var parameter in parameters)
                    command.Parameters.Add(parameter);

                using SqlDataAdapter adapter = new(command);
                await Task.Run(() => adapter.Fill(table));
            }

            return table;
        }

        private async Task<MutationResult> NonQuery(string storedProcedure, List<SqlParameter> parameters)
            => await NonQuery(storedProcedure, parameters, null);

        private async Task<MutationResult> NonQuery(string storedProcedure, List<SqlParameter> parameters, Guid? mutationId)
        {
            MutationResult result = new();

            using (SqlConnection connection = new(_connectionString))
            {
                using SqlCommand command = new(storedProcedure, connection) { CommandType = CommandType.StoredProcedure };

                if (mutationId.HasValue)
                    command.Parameters.Add(new SqlParameter("@MUT_ID", SqlDbType.UniqueIdentifier) { Value = mutationId });

                command.Parameters.Add(new SqlParameter("@OUT_ERROR", SqlDbType.Int) { Direction = ParameterDirection.Output });
                command.Parameters.Add(new SqlParameter("@OUT_IDENTITY", SqlDbType.UniqueIdentifier) { Direction = ParameterDirection.Output });
                command.Parameters.Add(new SqlParameter("@OUT_MESSAGE", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });

                foreach (var parameter in parameters)
                    command.Parameters.Add(parameter);

                connection.Open();

                try
                {
                    await command.ExecuteNonQueryAsync();
                    result.ErrorCode = int.TryParse(command.Parameters["@OUT_ERROR"].Value.ToString(), out var outErrorCode) ? outErrorCode : 0;
                    result.Identifier = Guid.TryParse(command.Parameters["@OUT_IDENTITY"].Value.ToString(), out var outIdentity) ? outIdentity : Guid.Empty;
                    result.Message = command.Parameters["@OUT_MESSAGE"].Value.ToString();
                }
                catch
                {

                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }

            return result;
        }
    }
}