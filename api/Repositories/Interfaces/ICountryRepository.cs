using Adventour.Api.Responses.Country;

namespace Adventour.Api.Repositories.Interfaces
{
    public interface ICountryRepository
    {
        IEnumerable<CountryResponse> GetCountries(int page, int pageSize, string continentName);
        CountryResponse GetCountry(string countryCode);
    }
}
