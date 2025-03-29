using Adventour.Api.Builders.Interfaces;
using Adventour.Api.Services.Database;
using CloudinaryDotNet.Core;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
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
            service.StoredProcedure = name;
            return this;
        }

        public QueryServiceBuilder WithParameter(string name, object value)
        {
            service.Parameters.Add(name, value);
            return this;
        }

        public QueryServiceBuilder WithOutputParameter(string name, DbType type)
        {
            service.Parameters.Add(name, dbType: type, direction: ParameterDirection.Output);
            return this;
        }

        public IDatabaseConnectionService Build()
        {
            var clonedService = service.CloneService();
            service.Parameters = new DynamicParameters();
            return clonedService;
        }

    }
}
