using Dapper;

namespace Adventour.Api.Responses.Country
{
    public class CountryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContinentName { get; set; }
    }
}