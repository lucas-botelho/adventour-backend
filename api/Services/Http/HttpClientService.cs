
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Adventour.Api.Services.Http
{
    public class HttpClientService<TResponse> : IHttpClientService<TResponse>
    {
        public string Url { get; set; }
        public IDictionary<string, string> Headers { get; set; }

        public async Task<TResponse> Get()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, this.Url);

            AddRequestHeaders(request);

            TResponse result = default; 

            using(var httpClient = new HttpClient())
            {
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<TResponse>(content);
                }
                else
                {
                    // Log error
                }
            }

            return result;
        }

        private void AddRequestHeaders(HttpRequestMessage request)
        {
            foreach (var header in Headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }
    }
}