using Dapper;

namespace Adventour.Api.Models.Country
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContinentName { get; set; }
    }
}