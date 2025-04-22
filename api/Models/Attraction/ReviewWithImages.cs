using Adventour.Api.Models.Database;

namespace Adventour.Api.Models.Attraction
{
    public class ReviewWithImages
    {
        public Adventour.Api.Models.Database.Review Review { get; set; }
        public List<ReviewImages> Images { get; set; }
    }
}