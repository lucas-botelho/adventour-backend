using Adventour.Api.Responses.Country;

namespace Adventour.Api.Repositories.Interfaces
{
    public interface ICountryRepository
    {
        CountryResponse GetCountry(string countryCode);
    }
}
