using System.Net.Http;
using System.Text;
using System.Text.Json;
using Adventour.Api.Exceptions;
using Adventour.Api.Responses.DistanceCalculation;
using Adventour.Api.Services.DistanceCalculation.Interfaces;
using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Errors.Model;

namespace Adventour.Api.Services.DistanceCalculation
{
    public class TomTomService : ITomTomService
    {
        private readonly HttpClient _httpClient;
        private readonly string apiKey;

        public TomTomService(IConfiguration configuration, HttpClient httpClient)
        {
            _httpClient = httpClient;
            apiKey = configuration["TOMTOM_API_KEY"]
                      ?? throw new Exception("TomTom API key not found.");
        }

        public async Task<string> GeocodeAsync(string address)
        {
            var encoded = Uri.EscapeDataString(address);
            var url = $"https://api.tomtom.com/search/2/geocode/{encoded}.json?key={_apiKey}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> ReverseGeocodeAsync(double lat, double lon)
        {
            var url = $"https://api.tomtom.com/search/2/reverseGeocode/{lat},{lon}.json?key={_apiKey}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<DistanceResult> GetDistanceInMetersAsync(double originLat, double originLon, double destLat, double destLon)
        {
            try
            {
                var url = $"https://api.tomtom.com/routing/matrix/2?key={apiKey}";

                var body = new
                {
                    origins = new[] { new { point = new { latitude = originLat, longitude = originLon } } },
                    destinations = new[] { new { point = new { latitude = destLat, longitude = destLon } } },
                    metrics = new[] { "distance" },
                    travelMode = "car"
                };

                var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new BadRequestException("Erro ao comunicar com a API da TomTom.");
                }

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);

                var length = doc.RootElement
                    .GetProperty("matrix")[0][0]
                    .GetProperty("routeSummary")
                    .GetProperty("lengthInMeters")
                    .GetInt32();

                return new DistanceResult
                {
                    DistanceInMeters = length
                };
            }
            catch (JsonException ex)
            {
                throw new BadRequestException("Erro ao interpretar a resposta da API da TomTom.");
            }
            catch (Exception)
            {
                throw;
            }
        }



    }
}
