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
        IEnumerable<AttractionDetails> GetBaseAttractionData(string countryCode, Person userId);
        Attraction? GetAttractionWithImages(int id);
        public IEnumerable<AttractionInfo>? GetAttractionInfo(int id);
        bool AddReview(int attractionId, AddReviewRequest data);
        IEnumerable<ReviewWithImages> GetAttractionReviews(int attractionId);
        IEnumerable<FavoritedAttractionDetails> GetFavorites(string oAuthId);
        Attraction GetAttraction(int id);
        bool AddAttraction(AddAttractionRequest data);
        List<InfoTypeResponse> GetInfoTypes();
        bool DeleteAttraction(int attractionId);
        Task<List<BackOfficeAttractionDetails>> GetAllAttractionsForAdminAsync();

    }
}
