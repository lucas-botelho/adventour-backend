using Adventour.Api.Builders.Interfaces;
using Adventour.Api.Services.Database;
using Dapper;
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
            service.StoredProcedure = name; ;
            return this;
        }

        public QueryServiceBuilder WithParameter(string name, object value)
        {
            service.Parameters.Add(name, value);
            return this;
        }

        public IDatabaseConnectionService Build()
        {
            return service;
        }
    }
}
