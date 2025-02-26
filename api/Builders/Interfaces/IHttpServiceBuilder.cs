using Adventour.Api.Services.Http;

namespace Adventour.Api.Builders.Interfaces
{
    public interface IHttpServiceBuilder<T>
    {
        IHttpClientService<T> Build();
        IHttpServiceBuilder<T> WithEndpoint(string endpoint);
        IHttpServiceBuilder<T> WithHeaders(string header, string value);
        IHttpServiceBuilder<T> WithQueryParameters(string parameter, string value);
        
    }
}
