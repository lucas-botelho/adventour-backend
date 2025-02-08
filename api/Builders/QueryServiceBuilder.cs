using Adventour.Api.Builders.Interfaces;
using Adventour.Api.Services.Database;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Adventour.Api.Builders
{
    public class QueryServiceBuilder : IQueryServiceBuilder
    {
        private readonly IDatabaseConnectionService service;

        public QueryServiceBuilder(IDatabaseConnectionService service)
        {
            this.service = service;
        }

        public QueryServiceBuilder WithStoredProcedure(string name)
        {
            service.Command = new SqlCommand(name, service.Connection);
            service.Command.CommandType = CommandType.StoredProcedure;
            return this;
        }

        public QueryServiceBuilder WithParameter(string name, object value)
        {
            service.Command.Parameters.AddWithValue(name, value);
            return this;
        }

        public T Execute <T>()
        {
            return service.ExecuteScalar<T>();
        }
    }
}
