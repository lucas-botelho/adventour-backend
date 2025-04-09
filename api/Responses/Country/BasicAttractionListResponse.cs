using Adventour.Api.Models;

namespace Adventour.Api.Responses.Country
{
    public class BasicAttractionListResponse
    {

        public BasicAttractionListResponse(IEnumerable<Attraction> Attractions)
        {
            this.Attractions = Attractions.ToList();
        }

        public IEnumerable<Attraction> Attractions{ get; set; }
    }
}
