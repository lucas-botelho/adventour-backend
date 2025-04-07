using Adventour.Api.Builders.Interfaces;
using Adventour.Api.Constants.Database;
using Adventour.Api.Data;
using Adventour.Api.Models;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Responses.Country;
using Google.Apis.Upload;
using System.Diagnostics.Metrics;

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
                var allCountries = db.Country
                    .Where(c => c.ContinentName.ToLower().Equals(continentName.ToLower()) && !string.IsNullOrEmpty(c.Svg))
                    .OrderBy(c => c.Name)
                    .ToList();

                int selectedIndex = allCountries.FindIndex(c => c.Code.Equals(selectedCountryCode.ToUpper()));


                if (selectedIndex == -1)
                {
                    logger.LogError($"{logHeader} Country code not found.");
                    return null;
                }

                total = allCountries.Count;
                if (page >= 0)
                {
                    // Positive page (next countries)
                    return allCountries
                        .Skip(selectedIndex + page * pageSize)
                        .Take(pageSize)
                        .ToList();
                }

                // Nagative Page (previous countries)
                int startIndex = Math.Max(0, selectedIndex + page * pageSize); 
                int count = Math.Min(pageSize, selectedIndex - startIndex);

                return allCountries
                    .Skip(startIndex)
                    .Take(count)
                    .ToList();
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
                return null;
            }
        }


    }
}
