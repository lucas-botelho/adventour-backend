using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;

namespace Adventour.Api.Services.Database
{
    public class MsSqlService : IDatabaseService
    {

        public SqlConnection Connection { get; set; }
        public DynamicParameters Parameters { get; set; }
        public string StoredProcedure { get; set; }

        private const string logHeader = "## DbConnectionService ##: ";
        private readonly ILogger<MsSqlService> logger;

        public MsSqlService(ILogger<MsSqlService> logger)
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
