using System.Net.Http;
using System.Text.Json;
using System.Text;
using Adventour.Api.Responses.DistanceCalculation;
using Adventour.Api.Models.Geolocation;

namespace Adventour.Api.Services.DistanceCalculation.Interfaces
{
    public interface IGeoLocationService
    {
        Task<GeocodeResult> AddressToGeoCode(string address);
        Task<string> GeoCodeToAddress(double lat, double lon);
        Task<DistanceResult> GetDistanceInMetersAsync(double originLat, double originLon, double destLat, double destLon);

    }

}
