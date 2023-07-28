using System.Data;
using System.Data.SqlClient;

namespace DGBCommerce.Data
{
    public class DataAccessLayer
    {
        private readonly string _connectionString;

        public DataAccessLayer(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public DataTable GetDeliveryMethod(Guid? id, string? name)
            => Get("SP_GET_DeliveryMethod", new List<SqlParameter>() {
                new SqlParameter("DLM_ID", SqlDbType.UniqueIdentifier){Value = id },
                new SqlParameter("DLM_NAME", SqlDbType.NVarChar) {Value = name }
            });

        public DataTable GetShop(Guid? id, string? name, string? subDomain)
            => Get("SP_GET_Shop", new List<SqlParameter>() {
                new SqlParameter("SHP_ID", SqlDbType.UniqueIdentifier){Value = id },
                new SqlParameter("SHP_NAME", SqlDbType.NVarChar) {Value = name },
                new SqlParameter("SHP_SUBDOMAIN", SqlDbType.NVarChar) {Value = subDomain }
            });

        private DataTable Get(string storedProcedure, List<SqlParameter> parameters)
        {
            DataTable table = new();

            using (SqlConnection connection = new(this._connectionString))
            {
                using SqlCommand command = new(storedProcedure, connection) { CommandType = CommandType.StoredProcedure };
                foreach (var parameter in parameters)
                    command.Parameters.Add(parameters);

                using SqlDataAdapter adapter = new(command);
                adapter.Fill(table);
            }

            return table;
        }
    }
}