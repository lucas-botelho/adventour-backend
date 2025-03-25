using Adventour.Api.Builders.Interfaces;
using Adventour.Api.Constants.Database;
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

        public CountryRepository(IQueryServiceBuilder queryServiceBuilder, ILogger<CountryRepository> logger)
        {
            this.queryServiceBuilder = queryServiceBuilder;
            this.logger = logger;

        }
        public CountryResponse GetCountry(string countryCode)
		{
			try
			{
				var dbService = queryServiceBuilder.WithStoredProcedure(StoredProcedures.GetCountryByCode)
                    .WithParameter(StoredProcedures.Parameters.Code, countryCode)
                    .Build();

				var country = dbService.QuerySingle<CountryResponse>();

				return country is not null ? country : new CountryResponse();
            }
			catch (Exception ex)
			{
				logger.LogError($"{logHeader} {ex.Message}");
                throw;
			}
		}

        public IEnumerable<CountryResponse> GetCountries(int page, int pageSize, string continentName)
        {
			try
			{
                var dbService = queryServiceBuilder.WithStoredProcedure(StoredProcedures.GetCountriesByContinent)
                    .WithParameter(StoredProcedures.Parameters.PageNumber, page)
                    .WithParameter(StoredProcedures.Parameters.FetchRows, pageSize)
                    .WithParameter(StoredProcedures.Parameters.ContinentName, continentName)
                    .Build();

                var countries = dbService.QueryMultiple<CountryResponse>();

                return countries is not null ? countries : new List<CountryResponse>();

            }
            catch (Exception ex)
			{
                logger.LogError($"{logHeader} {ex.Message}");
                throw;
            }
        }
    }
}
