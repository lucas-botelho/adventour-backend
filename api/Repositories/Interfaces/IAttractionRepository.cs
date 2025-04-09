
using Adventour.Api.Models.Attractions;

namespace Adventour.Api.Repositories.Interfaces
{
    public interface IAttractionRepository
    {
        bool AddToFavorites(int attractionId, string userId);
        IEnumerable<BasicAttractionDetails> GetBaseAttractionData(string countryCode, string userId);

    }
}
