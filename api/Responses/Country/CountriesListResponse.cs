namespace Adventour.Api.Responses.Country
{
    public class CountriesListResponse
    {
        public CountriesListResponse(IEnumerable<CountryResponse> countries)
        {
            this.Countries = countries;
        }
        public IEnumerable<CountryResponse> Countries { get; set; }
    }
}
