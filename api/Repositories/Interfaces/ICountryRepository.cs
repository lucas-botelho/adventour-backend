using Adventour.Api.Models;
using Adventour.Api.Responses.Country;

namespace Adventour.Api.Repositories.Interfaces
{
    public interface ICountryRepository
    {
        IEnumerable<Country>? GetCountries(string continentName, string selectedCountryCode, int pageSize, int page, out int total);
        Country? GetCountry(string countryCode);
    }
}
