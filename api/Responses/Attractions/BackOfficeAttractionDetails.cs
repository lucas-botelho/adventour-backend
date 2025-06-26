using Adventour.Api.Models.Database;

namespace Adventour.Api.Responses.Attractions
{
    public class BackOfficeAttractionDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public IEnumerable<AttractionImages> AttractionImages { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public double Rating { get; set; }
    }
}
