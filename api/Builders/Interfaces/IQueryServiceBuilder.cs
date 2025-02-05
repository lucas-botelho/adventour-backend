
namespace Adventour.Api.Builders.Interfaces
{
    public interface IQueryServiceBuilder
    {
        T Execute<T>();
        QueryServiceBuilder WithParameter(string name, object value);
        QueryServiceBuilder WithStoredProcedure(string name);
    }
}