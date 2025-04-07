using Adventour.Api.Models;
using Adventour.Api.Responses.Country;

namespace Adventour.Api.Repositories.Interfaces
{
    public interface ICountryRepository
    {
        IEnumerable<Country>? GetCountries(int page, int pageSize, string continentName, string selectedCountryCode);
        Country? GetCountry(string countryCode);
    }
}
