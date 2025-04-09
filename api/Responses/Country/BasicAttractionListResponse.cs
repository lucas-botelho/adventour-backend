using Adventour.Api.Models.Attractions;
using Adventour.Api.Models.Database;

namespace Adventour.Api.Responses.Country
{
    public class BasicAttractionListResponse
    {

        public BasicAttractionListResponse(IEnumerable<BasicAttractionDetails> Attractions)
        {
            this.Attractions = Attractions.ToList();
        }

        public IEnumerable<BasicAttractionDetails> Attractions{ get; set; }
    }
}
