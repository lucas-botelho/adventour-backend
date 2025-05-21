using System.Text;
using System.Text.Json;
using Adventour.Api.Exceptions;
using Adventour.Api.Models.Geolocation;
using Adventour.Api.Responses.DistanceCalculation;
using Adventour.Api.Services.DistanceCalculation.Interfaces;
using Microsoft.Extensions.Logging;

namespace Adventour.Api.Services.DistanceCalculation
{
    public class TomTomService : IGeoLocationService
    {
        private readonly HttpClient _httpClient;
        private readonly string apiKey;
        private readonly ILogger<TomTomService> logger;

        public TomTomService(IConfiguration configuration, HttpClient httpClient, ILogger<TomTomService> logger)
        {
            _httpClient = httpClient;
            apiKey = Environment.GetEnvironmentVariable("TOMTOM_API_KEY");
            this.logger = logger;
        }

        //public async Task<string> AddressToGeoCode(string address)
        //{
        //    var encoded = Uri.EscapeDataString(address);
        //    var url = $"https://api.tomtom.com/search/2/geocode/{encoded}.json?key={apiKey}";

        //    var response = await _httpClient.GetAsync(url);
        //    response.EnsureSuccessStatusCode();

        //    return await response.Content.ReadAsStringAsync();
        //}

        public async Task<string> GeoCodeToAddress(double lat, double lon)
        {
            var url = $"https://api.tomtom.com/search/2/reverseGeocode/{lat},{lon}.json?key={apiKey}";

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
                    destinations = new[] { new { point = new { latitude = destLat, longitude = destLon } } }
                };

                var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new NotFoundException("Error communicating with TomTom API.");
                }

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);

                var length = doc.RootElement
                    .GetProperty("data")[0]
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
                throw new NotFoundException("Error interpreting TomTom API's response.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GeocodeResult> AddressToGeoCode(string address)
        {
            var encoded = Uri.EscapeDataString(address);
            var url = $"https://api.tomtom.com/search/2/geocode/{encoded}.json?key={apiKey}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var position = doc.RootElement
                .GetProperty("results")[0]
                .GetProperty("position");

            var lat = position.GetProperty("lat").GetDouble();
            var lon = position.GetProperty("lon").GetDouble();

            return new GeocodeResult
            {
                Latitude = lat,
                Longitude = lon
            };
        }

        public async Task<DistanceResult> GetDistanceInMetersAsync(double originLat, double originLon, string address)
        {
            var geocodeDestination = await AddressToGeoCode(address);

            if (geocodeDestination != null)
            {
                return await GetDistanceInMetersAsync(originLat, originLon, geocodeDestination.Latitude, geocodeDestination.Longitude);
            }
            else
            {
                throw new NotFoundException("Error communicating with TomTom API.");
            }
        }
    }
}
