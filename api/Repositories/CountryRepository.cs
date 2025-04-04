using Adventour.Api.Builders.Interfaces;
using Adventour.Api.Constants.Database;
using Adventour.Api.Data;
using Adventour.Api.Models;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Responses.Country;
using System.Diagnostics.Metrics;

namespace Adventour.Api.Repositories
{
    public class CountryRepository : ICountryRepository
	{
		private readonly IQueryServiceBuilder queryServiceBuilder;
        private readonly ILogger<CountryRepository> logger;
		private const string logHeader = "## CountryRepository ##: ";
        private readonly AdventourContext db;

        public CountryRepository(IQueryServiceBuilder queryServiceBuilder, ILogger<CountryRepository> logger, AdventourContext db)
        {
            this.queryServiceBuilder = queryServiceBuilder;
            this.logger = logger;
            this.db = db;
        }
        public Country? GetCountry(string countryCode)
        {
            try
            {
                return db.Country.Where(c => c.Code.Equals(countryCode, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
                throw;
            }
        }

        public IEnumerable<Country>? GetCountries(int page, int pageSize, string continentName, string selectedCountryCode)
        {
			try
			{
                //Get all countries except the selected country
                return db.Country.Where(c => 
                c.ContinentName.Equals(continentName, StringComparison.OrdinalIgnoreCase) 
                && c.Code.Equals(selectedCountryCode, StringComparison.OrdinalIgnoreCase))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            catch (Exception ex)
			{
                logger.LogError($"{logHeader} {ex.Message}");
                throw;
            }
        }
    }
}
