using System.Net.Http;
using System.Text.Json;
using Adventour.Api.Services.DistanceCalculation.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Adventour.Api.Services.DistanceCalculation
{
    public class TomTomService : ITomTomService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public TomTomService(IConfiguration configuration, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiKey = configuration["TOMTOM_API_KEY"]
                      ?? throw new Exception("TomTom API key not found.");
        }

        public async Task<string> GetRouteAsync(double startLat, double startLon, double endLat, double endLon)
        {
            var url = $"https://api.tomtom.com/routing/1/calculateRoute/{startLat},{startLon}:{endLat},{endLon}/json?key={_apiKey}&travelMode=car";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
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
    }
}
