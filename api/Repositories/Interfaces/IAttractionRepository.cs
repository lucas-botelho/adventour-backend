using Adventour.Api.Requests.Attraction;
using Adventour.Api.Responses.Attraction;

namespace Adventour.Api.Repositories.Interfaces
{
    public interface IAttractionRepository
    {
        Attraction GetAttractionById(int attractionId);
        int AddAttraction(AddAttractionRequest request);
    }
}
