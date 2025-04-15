using System.Net.Http;
using System.Text.Json;
using System.Text;
using Adventour.Api.Responses.DistanceCalculation;

namespace Adventour.Api.Services.DistanceCalculation.Interfaces
{
    public interface ITomTomService
    {
        Task<string> GeocodeAsync(string address);
        Task<string> ReverseGeocodeAsync(double lat, double lon);
        Task<DistanceResult> GetDistanceInMetersAsync(double originLat, double originLon, double destLat, double destLon);

    }

}
