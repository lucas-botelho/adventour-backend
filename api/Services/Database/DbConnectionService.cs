using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Adventour.Api.Services.Database
{
    public class DbConnectionService : IDatabaseConnectionService
    {

        public SqlConnection Connection { get; set; }
        public SqlCommand Command { get; set; }

        private const string logHeader = "## DbConnectionService ##: ";
        private readonly ILogger<DbConnectionService> logger;
        public DbConnectionService(ILogger<DbConnectionService> logger)
        {
            Connection = new SqlConnection(Environment.GetEnvironmentVariable("CONNECTION_STRING")!);
            this.logger = logger;
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
                logger.LogError($"{logHeader} {ex.Message}");
                throw;
            }

        }
    }
}
