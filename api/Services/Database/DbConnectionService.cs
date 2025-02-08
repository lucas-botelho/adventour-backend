using Microsoft.Data.SqlClient;

namespace Adventour.Api.Services.Database
{
    public class DbConnectionService : IDatabaseConnectionService
    {

        public SqlConnection Connection { get; set; }
        public SqlCommand Command { get; set; }

        public DbConnectionService()
        {
            Connection = new SqlConnection(Environment.GetEnvironmentVariable("CONNECTION_STRING")!);
        }

        public T ExecuteScalar<T>()
        {
            try
            {
                Connection.Open();
                var result = (T)Command.ExecuteScalar();
                Connection.Close();

                Command.Dispose();
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
