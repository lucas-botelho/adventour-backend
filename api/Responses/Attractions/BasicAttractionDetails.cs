using Adventour.Api.Models.Database;

namespace Adventour.Api.Responses.Attractions
{
    public class BasicAttractionDetails
    {
        public int Id { get; set; }
        public bool IsFavorited { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public IEnumerable<AttractionImages> AttractionImages { get; set; }

    }
}
