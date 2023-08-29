using DGBCommerce.Domain.Interfaces;
using System.Data;
using Microsoft.Data.SqlClient;
using DGBCommerce.Domain.Parameters;
using DGBCommerce.Domain;
using DGBCommerce.Domain.Models;
using DGBCommerce.Domain.Enums;

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
                new SqlParameter("@CAT_SHOP", SqlDbType.UniqueIdentifier) { Value = category.Shop.Id },
                new SqlParameter("@CAT_PARENT", SqlDbType.UniqueIdentifier) { Value = category.Parent?.Id },
                new SqlParameter("@CAT_NAME", SqlDbType.NVarChar, 255) { Value = category.Name },
                new SqlParameter("@CAT_SORTORDER", SqlDbType.Int) { Value = category.SortOrder }
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
                new SqlParameter("@MER_FIRST_NAME", SqlDbType.UniqueIdentifier) { Value = merchant.FirstName },
                new SqlParameter("@MER_LAST_NAME", SqlDbType.UniqueIdentifier) { Value = merchant.LastName }
            }, mutationId);

        public async Task<MutationResult> CreateShop(Shop shop, Guid mutationId)
            => await NonQuery("SP_MUTATE_Shop", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Create },
                new SqlParameter("@SHP_MERCHANT", SqlDbType.UniqueIdentifier) { Value = shop.Merchant.Id },
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

        public async Task<DataTable> GetNewsMessages(GetNewsMessagesParameters parameters)
            => await Get("SP_GET_NewsMessages", new List<SqlParameter>() {
                new SqlParameter("@NWS_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("@NWS_TITLE", SqlDbType.NVarChar) { Value = parameters.Title },
                new SqlParameter("@NWS_DATE_FROM", SqlDbType.DateTime) { Value = parameters.DateFrom },
                new SqlParameter("@NWS_DATE_UNTIL", SqlDbType.DateTime) { Value = parameters.DateUntil }
            });

        public async Task<DataTable> GetMerchantForForgotPassword(GetMerchantForForgotPasswordParameters parameters)
            => await Get("SP_GET_MerchantForForgotPassword", new List<SqlParameter>() {
                new SqlParameter("@MER_EMAIL_ADDRESS", SqlDbType.VarChar) { Value = parameters.EmailAddress }
            });

        public async Task<DataTable> GetMerchantForLogin(GetMerchantForLoginParameters parameters)
            => await Get("SP_GET_MerchantForLogin", new List<SqlParameter>() {
                new SqlParameter("@MER_EMAIL_ADDRESS", SqlDbType.VarChar) { Value = parameters.EmailAddress },
                new SqlParameter("@MER_PASSWORD", SqlDbType.VarChar) { Value = parameters.Password },
            });

        public async Task<DataTable> GetMerchants(GetMerchantsParameters parameters)
            => await Get("SP_GET_Merchants", new List<SqlParameter>() {
                new SqlParameter("MER_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("MER_EMAIL_ADDRESS", SqlDbType.VarChar) { Value = parameters.EmailAddress },
                new SqlParameter("MER_PASSWORD", SqlDbType.VarChar) { Value = parameters.Password },
                new SqlParameter("MER_FIRST_NAME", SqlDbType.NVarChar) { Value = parameters.FirstName },
                new SqlParameter("MER_LAST_NAME", SqlDbType.NVarChar) { Value = parameters.LastName }
            });

        public async Task<DataTable> GetShops(GetShopsParameters parameters)
            => await Get("SP_GET_Shops", new List<SqlParameter>() {
                new SqlParameter("SHP_ID", SqlDbType.UniqueIdentifier){ Value = parameters.Id },
                new SqlParameter("SHP_NAME", SqlDbType.NVarChar) { Value = parameters.Name },
                new SqlParameter("SHP_SUBDOMAIN", SqlDbType.NVarChar) { Value = parameters.SubDomain }
            });
        #endregion

        #region Update
        public async Task<MutationResult> UpdateCategory(Category category, Guid mutationId)
            => await NonQuery("SP_MUTATE_Category", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Update },
                new SqlParameter("@CAT_ID", SqlDbType.UniqueIdentifier) { Value = category.SortOrder },
                new SqlParameter("@CAT_SHOP", SqlDbType.UniqueIdentifier) { Value = category.Shop.Id },
                new SqlParameter("@CAT_PARENT", SqlDbType.UniqueIdentifier) { Value = category.Parent?.Id },
                new SqlParameter("@CAT_NAME", SqlDbType.NVarChar, 255) { Value = category.Name },
                new SqlParameter("@CAT_SORTORDER", SqlDbType.Int) { Value = category.SortOrder }
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

        public async Task<MutationResult> UpdateShop(Shop shop, Guid mutationId)
            => await NonQuery("SP_MUTATE_Shop", new List<SqlParameter>() {
                new SqlParameter("@COMMAND", SqlDbType.TinyInt) { Value = MutationType.Update },
                new SqlParameter("@SHP_ID", SqlDbType.UniqueIdentifier) { Value = shop.Id },
                new SqlParameter("@SHP_MERCHANT", SqlDbType.UniqueIdentifier) { Value = shop.Merchant.Id },
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

        private async Task<MutationResult> NonQuery(string storedProcedure, List<SqlParameter> parameters, Guid mutationId)
        {
            MutationResult result = new();

            using (SqlConnection connection = new(_connectionString))
            {
                using SqlCommand command = new(storedProcedure, connection) { CommandType = CommandType.StoredProcedure };
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