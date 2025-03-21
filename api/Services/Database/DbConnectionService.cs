using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;

namespace Adventour.Api.Services.Database
{
    public class DbConnectionService : IDatabaseConnectionService
    {

        public SqlConnection Connection { get; set; }
        public DynamicParameters Parameters { get; set; }
        public string StoredProcedure { get; set; }

        private const string logHeader = "## DbConnectionService ##: ";
        private readonly ILogger<DbConnectionService> logger;

        public DbConnectionService(ILogger<DbConnectionService> logger)
        {
            this.Parameters = new DynamicParameters();
            Connection = new SqlConnection(Environment.GetEnvironmentVariable("CONNECTION_STRING")!);
            this.logger = logger;
        }

        public IEnumerable<T> QueryMultiple<T>()
        {
            try
            {
                Connection.Open();
                var result = Connection.Query<T>(this.StoredProcedure, this.Parameters, commandType: CommandType.StoredProcedure).ToList();
                Connection.Close();
                return result is not null ? result : default!;
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
            }

            return default;
        }

        public T QuerySingle<T>()
        {
            try
            {
                return this.QueryMultiple<T>().FirstOrDefault()!;
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
                throw;
            }
        }

        public T GetOutputParameter<T>(string name)
        {
            return (T)Parameters.Get<T>(name);
        }

        public T InsertSingleWithOutput<T>(string outputParamName)
        {
            try
            {
                QuerySingle<int>();

                return GetOutputParameter<T>(outputParamName);
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
                throw;
            }
        }

        public bool Update()
        {
            try
            {
                Connection.Open();

                var result = Connection.Execute(this.StoredProcedure, this.Parameters, commandType: CommandType.StoredProcedure);

                Connection.Close();

                return result > 0;
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
            }

            return false;
        }
    }
}
