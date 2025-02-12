using Adventour.Api.Builders.Interfaces;
using Adventour.Api.Constants.Database;
using Adventour.Api.Models.Country;
using Adventour.Api.Repositories.Interfaces;

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
        public Country GetCountry(string countryCode)
		{
			try
			{
				var dbService = queryServiceBuilder.WithStoredProcedure(StoredProcedures.GetCountryByCode)
                    .WithParameter(StoredProcedures.Parameters.Code, countryCode)
                    .Build();

				var country = dbService.QuerySingle<Country>();

				return country is not null ? country : new Country();
            }
			catch (Exception ex)
			{
				logger.LogError($"{logHeader} {ex.Message}");
                throw;
			}
		}
	}
}
