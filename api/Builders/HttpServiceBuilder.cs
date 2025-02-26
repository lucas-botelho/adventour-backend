using Adventour.Api.Builders.Interfaces;
using Adventour.Api.Services.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Adventour.Api.Builders
{
    public class HttpServiceBuilder<TResponse> : IHttpServiceBuilder<TResponse>
    {
        private readonly IHttpClientService<TResponse> service;
        private string url;
        private bool hasQueryParameters = false;

        public HttpServiceBuilder(IHttpClientService<TResponse> service)
        {
            this.service = service;
        }

        public IHttpClientService<TResponse> Build()
        {
            service.Url = url;
            return service;
        }

        public IHttpServiceBuilder<TResponse> WithEndpoint(string url)
        {
            this.url = url;
            return this;
        }

        public IHttpServiceBuilder<TResponse> WithHeaders(string header, string value)
        {
            service.Headers.Add(header, value);
            return this;
        }

        public IHttpServiceBuilder<TResponse> WithQueryParameters(string parameter, string value)
        {
            if (!hasQueryParameters)
            {
                url += '?';
                hasQueryParameters = true;
            }

            url += $"&{parameter}={value}";

            return this;
        }

    }
}
