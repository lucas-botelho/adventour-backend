using Adventour.Api.Data;
using Adventour.Api.Models.Database;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Responses.Country;

namespace Adventour.Api.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly ILogger<CountryRepository> logger;
        private const string logHeader = "## CountryRepository ##: ";
        private readonly AdventourContext db;

        public CountryRepository(ILogger<CountryRepository> logger, AdventourContext db)
        {
            this.logger = logger;
            this.db = db;
        }
        public Country? GetCountry(string countryCode)
        {
            try
            {
                return db.Country.Where(c => c.Code.Equals(countryCode.ToUpper())).FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
                return null;
            }
        }

        public IEnumerable<Country>? GetCountries(string continentName, string selectedCountryCode, int pageSize, int page, out int total)
        {
            total = 0;

            try
            {
                var countriesByContinent = db.Country
                    .Where(c => c.ContinentName.ToLower().Equals(continentName.ToLower()) && !string.IsNullOrEmpty(c.Svg))
                    .OrderBy(c => c.Name)
                    .ToList();

                int selectedIndex = countriesByContinent.FindIndex(c => c.Code.Equals(selectedCountryCode.ToUpper()));

                if (selectedIndex == -1)
                {
                    logger.LogError($"{logHeader} Country code not found.");
                    return null;
                }

                total = countriesByContinent.Count;

                if (page >= 0)
                {
                    // Positive page (next countries)
                    return countriesByContinent
                        .Skip(selectedIndex + page * pageSize)
                        .Take(pageSize)
                        .ToList();
                }
                // Negative page (previous countries)
                int endIndex = selectedIndex;
                int startIndex = selectedIndex + page * pageSize;

                // If startIndex is outside the list bounds or after endIndex, return empty
                if (startIndex < 0 || startIndex >= endIndex)
                {
                    return new List<Country>();
                }

                // Ensure we don't go below 0
                int validStartIndex = Math.Max(0, startIndex);
                int count = Math.Min(pageSize, endIndex - validStartIndex);

                return countriesByContinent
                    .Skip(validStartIndex)
                    .Take(count)
                    .ToList();
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
                return null;
            }
        }

        public List<AllCountryDto> GetAllCountries()
        {
            try
            {
                return db.Country.Select(c => new AllCountryDto { Id = c.Id, Name = c.Name }).ToList();
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
                return null;
            }
        }
    }
}
