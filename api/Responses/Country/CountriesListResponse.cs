namespace Adventour.Api.Responses.Country
{
    using Adventour.Api.Models;
    public class CountriesListResponse
    {
        public CountriesListResponse(IEnumerable<Country> countries, int total = 0)
        {
            this.Total = total;
            this.Countries = countries;
        }

        public IEnumerable<Country> Countries { get; set; }
        public int Total { get; set; }
    }
}
