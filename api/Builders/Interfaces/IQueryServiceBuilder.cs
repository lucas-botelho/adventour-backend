
using Adventour.Api.Services.Database;

namespace Adventour.Api.Builders.Interfaces
{
    public interface IQueryServiceBuilder
    {
        IDatabaseService Build();
        QueryServiceBuilder WithParameter(string name, object value);
        QueryServiceBuilder WithStoredProcedure(string name);
    }
}