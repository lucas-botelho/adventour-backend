namespace Adventour.Api.Responses.Country
{
    using Adventour.Api.Models;
    public class CountriesListResponse
    {
        public CountriesListResponse(IEnumerable<Country> countries)
        {
            this.Countries = countries;
        }
        public IEnumerable<Country> Countries { get; set; }
    }
}
