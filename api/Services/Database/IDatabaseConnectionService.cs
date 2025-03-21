using Dapper;
using Microsoft.Data.SqlClient;

namespace Adventour.Api.Services.Database
{
    public interface IDatabaseConnectionService
    {
        DynamicParameters Parameters { get; set; }
        string StoredProcedure { get; set; }
        SqlConnection Connection { get; set; }
        T QuerySingle<T>();
        IEnumerable<T> QueryMultiple<T>();
        T GetOutputParameter<T>(string name);
        T InsertSingleWithOutput<T>(String name);
        bool Update();

    }

}
