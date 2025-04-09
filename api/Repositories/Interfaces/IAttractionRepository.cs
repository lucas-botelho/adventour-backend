using Adventour.Api.Models;

namespace Adventour.Api.Repositories.Interfaces
{
    public interface IAttractionRepository
    {
        IEnumerable<Attraction> GetBaseAttractionData(string countryCode);

    }
}
