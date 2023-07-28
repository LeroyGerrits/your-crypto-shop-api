using System.Data;
using System.Data.SqlClient;

namespace DGBCommerce.Data
{
    public class DataAccessLayer
    {
        private readonly string _connectionString;

        public DataAccessLayer(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<DataTable> GetDeliveryMethod(Guid? id, string? name)
            => await Get("SP_GET_DeliveryMethod", new List<SqlParameter>() {
                new SqlParameter("DLM_ID", SqlDbType.UniqueIdentifier){Value = id },
                new SqlParameter("DLM_NAME", SqlDbType.NVarChar) {Value = name }
            });

        public async Task<DataTable> GetShop(Guid? id, string? name, string? subDomain)
            => await Get("SP_GET_Shop", new List<SqlParameter>() {
                new SqlParameter("SHP_ID", SqlDbType.UniqueIdentifier){Value = id },
                new SqlParameter("SHP_NAME", SqlDbType.NVarChar) {Value = name },
                new SqlParameter("SHP_SUBDOMAIN", SqlDbType.NVarChar) {Value = subDomain }
            });

        private async Task<DataTable> Get(string storedProcedure, List<SqlParameter> parameters)
        {
            DataTable table = new();

            using (SqlConnection connection = new(_connectionString))
            {
                using SqlCommand command = new(storedProcedure, connection) { CommandType = CommandType.StoredProcedure };
                foreach (var parameter in parameters)
                    command.Parameters.Add(parameters);

                var reader = await command.ExecuteReaderAsync();
                table.Load(reader);
            }

            return table;
        }
    }
}