

using System.Data;
using Microsoft.Data.SqlClient;

namespace CRUD_Dapper.Data
{
    public class DapperContext: IDapperContext
    {
        private readonly IConfiguration _iconfiguration;
        private readonly string _connString;
        public DapperContext(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
            _connString = _iconfiguration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connString);
    }
}
