using Adventour.Api.Models.Attraction;
using Adventour.Api.Models.Database;
using Adventour.Api.Requests.Attraction;
using Adventour.Api.Responses.Attractions;

namespace Adventour.Api.Repositories.Interfaces
{
    public interface IAttractionRepository
    {
        bool AddToFavorites(int attractionId, string userId);
        bool RemoveFavorite(int attractionId, string userId);
        IEnumerable<BasicAttractionDetails> GetBaseAttractionData(string countryCode, string userId);
        Attraction? GetAttractionWithImages(int id);
        public IEnumerable<AttractionInfo>? GetAttractionInfo(int id);
        bool AddReview(int attractionId, AddReviewRequest data);
        IEnumerable<ReviewWithImages> GetAttractionReviews(int attractionId);
    }
}
