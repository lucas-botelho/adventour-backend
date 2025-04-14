namespace Adventour.Api.Services.DistanceCalculation.Interfaces
{
    public interface ITomTomService
    {
        Task<string> GetRouteAsync(double startLat, double startLon, double endLat, double endLon);
        Task<string> GeocodeAsync(string address);
        Task<string> ReverseGeocodeAsync(double lat, double lon);
    }

}
