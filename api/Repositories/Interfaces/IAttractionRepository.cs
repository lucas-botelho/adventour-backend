using Adventour.Api.Requests.Attraction;
using Adventour.Api.Responses.Attraction;
using Adventour.Api.Responses.City;

namespace Adventour.Api.Repositories.Interfaces
{
    public interface IAttractionRepository
    {
        Attraction GetAttractionById(int attractionId);
        int AddAttraction(AddAttractionRequest request);
        City GetCityById(int CityId);
        bool UpdateAttraction(UpdateAttractionRequest request);
        bool DeleteAttraction(int attractionId);
    }
}
