using Adventour.Api.Models.Database;

namespace Adventour.Api.Responses.Attractions
{
    internal class AttractionInfoResponse
    {
        public HashSet<AttractionInfoType> InfoTypes { get; set; }
        public IEnumerable<AttractionInfo> AttractionInfos { get; set; }
    }
}