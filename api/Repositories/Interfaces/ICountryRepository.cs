using Adventour.Api.Models.Country;

namespace Adventour.Api.Repositories.Interfaces
{
    public interface ICountryRepository
    {
        Country GetCountry(string countryCode);
    }
}
