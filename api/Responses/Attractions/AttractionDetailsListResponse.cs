using Adventour.Api.Responses.Attractions;

namespace Adventour.Api.Responses.Attractions
{
    public class AttractionDetailsListResponse
    {

        public AttractionDetailsListResponse(IEnumerable<AttractionDetails> Attractions)
        {
            this.Attractions = Attractions.ToList();
        }

        public IEnumerable<AttractionDetails> Attractions{ get; set; }
    }
}
