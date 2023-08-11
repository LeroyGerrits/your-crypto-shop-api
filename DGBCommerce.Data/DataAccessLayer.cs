﻿using DGBCommerce.Domain.Interfaces;
using System.Data;
using Microsoft.Data.SqlClient;
using DGBCommerce.Domain.Parameters;
using DGBCommerce.Domain;
using DGBCommerce.Domain.Models;

namespace DGBCommerce.Data
{
    public class DataAccessLayer : IDataAccessLayer
    {
        private readonly string _connectionString;

        public DataAccessLayer(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<MutationResult> CreateDeliveryMethod(DeliveryMethod deliveryMethod, Guid merchantId)
            => await NonQuery("SP_CREATE_DeliveryMethod", new List<SqlParameter>() {
                new SqlParameter("DLM_SHOP", SqlDbType.UniqueIdentifier) { Value = deliveryMethod.Shop.Id },
                new SqlParameter("DLM_NAME", SqlDbType.NVarChar, 255) { Value = deliveryMethod.Name },
                new SqlParameter("DLM_COSTS", SqlDbType.Decimal) { Value = deliveryMethod.Costs }
            }, merchantId);

        public async Task<MutationResult> CreateMerchant(Merchant merchant, Guid merchantId)
            => await NonQuery("SP_CREATE_Merchant", new List<SqlParameter>() {
                new SqlParameter("MER_FIRST_NAME", SqlDbType.UniqueIdentifier) { Value = merchant.FirstName },
                new SqlParameter("MER_LAST_NAME", SqlDbType.UniqueIdentifier) { Value = merchant.LastName }
            }, merchantId);

        public async Task<MutationResult> CreateShop(Shop shop, Guid merchantId)
            => await NonQuery("SP_CREATE_Merchant", new List<SqlParameter>() {
                new SqlParameter("SHP_MERCHANT", SqlDbType.UniqueIdentifier) { Value = shop.Merchant.Id },
                new SqlParameter("SHP_NAME", SqlDbType.NVarChar) { Value = shop.Name },
                new SqlParameter("SHP_SUBDOMAIN", SqlDbType.VarChar) { Value = shop.SubDomain }
            }, merchantId);

        public async Task<DataTable> GetCategories(GetCategoriesParameters parameters)
            => await Get("SP_GET_Categories", new List<SqlParameter>() {
                new SqlParameter("CAT_ID", SqlDbType.UniqueIdentifier){ Value = parameters.Id },
                new SqlParameter("CAT_SHOP", SqlDbType.UniqueIdentifier) { Value = parameters.ShopId },
                new SqlParameter("CAT_SHOP_MERCHANT", SqlDbType.UniqueIdentifier) { Value = parameters.MerchantId },
                new SqlParameter("CAT_PARENT", SqlDbType.UniqueIdentifier) { Value = parameters.ParentId },
                new SqlParameter("CAT_NAME", SqlDbType.NVarChar) { Value = parameters.Name }
            });

        public async Task<DataTable> GetDeliveryMethods(GetDeliveryMethodsParameters parameters)
            => await Get("SP_GET_DeliveryMethods", new List<SqlParameter>() {
                new SqlParameter("DLM_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("DLM_SHOP", SqlDbType.UniqueIdentifier) { Value = parameters.ShopId },
                new SqlParameter("DLM_NAME", SqlDbType.NVarChar) { Value = parameters.Name }
            });

        public async Task<DataTable> GetFaqCategories(GetFaqCategoriesParameters parameters)
            => await Get("SP_GET_FaqCategories", new List<SqlParameter>() {
                new SqlParameter("CAT_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("CAT_NAME", SqlDbType.NVarChar) { Value = parameters.Name }
            });

        public async Task<DataTable> GetFaqs(GetFaqsParameters parameters)
            => await Get("SP_GET_Faqs", new List<SqlParameter>() {
                new SqlParameter("FAQ_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("FAQ_CATEGORY", SqlDbType.UniqueIdentifier) { Value = parameters.CategoryId },
                new SqlParameter("FAQ_TITLE", SqlDbType.NVarChar) { Value = parameters.Title },
                new SqlParameter("FAQ_KEYWORDS", SqlDbType.NVarChar) { Value = parameters.Keywords },
            });

        public async Task<DataTable> GetNewsMessages(GetNewsMessagesParameters parameters)
            => await Get("SP_GET_NewsMessages", new List<SqlParameter>() {
                new SqlParameter("NWS_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("NWS_TITLE", SqlDbType.NVarChar) { Value = parameters.Title },
                new SqlParameter("NWS_DATE_FROM", SqlDbType.DateTime) { Value = parameters.DateFrom },
                new SqlParameter("NWS_DATE_UNTIL", SqlDbType.DateTime) { Value = parameters.DateUntil }
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

        private async Task<MutationResult> NonQuery(string storedProcedure, List<SqlParameter> parameters, Guid merchantId)
        {
            MutationResult result = new();

            using (SqlConnection connection = new(_connectionString))
            {
                using SqlCommand command = new(storedProcedure, connection) { CommandType = CommandType.StoredProcedure };
                command.Parameters.Add(new SqlParameter("@MUT_ID", SqlDbType.UniqueIdentifier) { Value = merchantId });
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