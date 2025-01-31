using Microsoft.Data.SqlClient;

namespace Adventour.Api.Services.Interfaces
{
    public interface IDatabaseConnectionService
    {
        public SqlConnection Connection { get; set; }
        public SqlCommand Command { get; set; }
        public T ExecuteScalar<T>();
    }

}
