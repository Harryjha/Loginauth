using System.Data;

namespace CRUD_Dapper.Data
{
    public interface IDapperContext
    {
        IDbConnection CreateConnection();
    }
}
