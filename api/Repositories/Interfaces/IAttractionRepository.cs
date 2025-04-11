using Adventour.Api.Models.Database;
using Adventour.Api.Responses.Attractions;

namespace Adventour.Api.Repositories.Interfaces
{
    public interface IAttractionRepository
    {
        bool AddToFavorites(int attractionId, string userId);
        bool RemoveFavorite(int attractionId, string userId);
        IEnumerable<BasicAttractionDetails> GetBaseAttractionData(string countryCode, string userId);
        Attraction GetAttraction(int id);
    }
}
