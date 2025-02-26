using System.Net.Http.Headers;

namespace Adventour.Api.Services.Http
{
    public interface IHttpClientService<T>
    {
        IDictionary<string, string> Headers { get; set; }
        string Url { get; set; }
        Task<T> Get();
    }
}
