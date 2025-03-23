using Adventour.Api.Builders.Interfaces;
using Adventour.Api.Constants.Database;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Responses.Itinerary;

namespace Adventour.Api.Repositories
{
    public class ItineraryRepository : IItineraryRepository
    {
        private readonly IQueryServiceBuilder queryServiceBuilder;
        private readonly ILogger<ItineraryRepository> logger;
        private const string logHeader = "## ItineraryRepository ##: ";

        public ItineraryRepository(IQueryServiceBuilder queryServiceBuilder, ILogger<ItineraryRepository> logger)
        {
            this.queryServiceBuilder = queryServiceBuilder;
            this.logger = logger;

        }
        public ItineraryResponse GetItineraryById(int itineraryId)
        {
            try
            {
                var dbService = queryServiceBuilder.WithStoredProcedure(StoredProcedures.GetItineraryById)
                    .WithParameter(StoredProcedures.Parameters.ItineraryId, itineraryId) 
                    .Build();

                var itinerary = dbService.QuerySingle<ItineraryResponse>();

                return itinerary is not null ? itinerary : null;
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
                throw;
            }
        }
    }
}