using Adventour.Api.Models.Attraction;

namespace Adventour.Api.Responses.Attractions
{
    public class ReviewWithImagesResponse
    {
        public ReviewWithImagesResponse(IEnumerable<ReviewWithImages> reviews, double averageRating)
        {
            this.Reviews = reviews;
            AverageRating = averageRating;
        }
        public IEnumerable<ReviewWithImages> Reviews { get; set; }
        public double AverageRating { get; set; }

    }
}
