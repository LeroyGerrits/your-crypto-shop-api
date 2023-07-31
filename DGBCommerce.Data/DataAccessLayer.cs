using DGBCommerce.Domain.Interfaces;
using System.Data;
using Microsoft.Data.SqlClient;
using DGBCommerce.Domain.Parameters;

namespace DGBCommerce.Data
{
    public class DataAccessLayer : IDataAccessLayer
    {
        private readonly string _connectionString;

        public DataAccessLayer(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<DataTable> GetDeliveryMethods(GetDeliveryMethodsParameters parameters)
            => await Get("SP_GET_DeliveryMethods", new List<SqlParameter>() {
                new SqlParameter("DLM_ID", SqlDbType.UniqueIdentifier){Value = parameters.Id },
                new SqlParameter("DLM_NAME", SqlDbType.NVarChar) {Value = parameters.Name }
            });

        public async Task<DataTable> GetFaqCategories(GetFaqCategoriesParameters parameters)
            => await Get("SP_GET_FaqCategories", new List<SqlParameter>() {
                new SqlParameter("CAT_ID", SqlDbType.UniqueIdentifier){ Value = parameters.Id },
                new SqlParameter("CAT_PARENT", SqlDbType.UniqueIdentifier){Value = parameters.ParentId },
                new SqlParameter("CAT_NAME", SqlDbType.NVarChar) {Value = parameters.Name }
            });

        public async Task<DataTable> GetFaqs(GetFaqsParameters parameters)
            => await Get("SP_GET_Faqs", new List<SqlParameter>() {
                new SqlParameter("FAQ_ID", SqlDbType.UniqueIdentifier){ Value = parameters.Id },
                new SqlParameter("FAQ_CATEGORY", SqlDbType.UniqueIdentifier){Value = parameters.CategoryId },
                new SqlParameter("FAQ_TITLE", SqlDbType.NVarChar) {Value = parameters.Title },
                new SqlParameter("FAQ_KEYWORDS", SqlDbType.NVarChar) {Value = parameters.Keywords },
            });

        public async Task<DataTable> GetNewsMessages(GetNewsMessagesParameters parameters)
            => await Get("SP_GET_NewsMessages", new List<SqlParameter>() {
                new SqlParameter("NWS_ID", SqlDbType.UniqueIdentifier) { Value = parameters.Id },
                new SqlParameter("NWS_TITLE", SqlDbType.NVarChar) { Value = parameters.Title },
                new SqlParameter("NWS_DATE_FROM", SqlDbType.DateTime) { Value = parameters.DateFrom },
                new SqlParameter("NWS_DATE_UNTIL", SqlDbType.DateTime) { Value = parameters.DateUntil }
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
    }
}