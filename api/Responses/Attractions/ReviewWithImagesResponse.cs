using Adventour.Api.Models.Attraction;

namespace Adventour.Api.Responses.Attractions
{
    public class ReviewWithImagesResponse
    {
        public ReviewWithImagesResponse(IEnumerable<ReviewWithImages> reviews)
        {
            this.Reviews = reviews;
        }
        public IEnumerable<ReviewWithImages> Reviews { get; set; }   
    }
}
