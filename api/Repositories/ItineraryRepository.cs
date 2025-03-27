using Adventour.Api.Builders;
using Adventour.Api.Builders.Interfaces;
using Adventour.Api.Constants.Database;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Responses.Day;
using Adventour.Api.Responses.Itinerary;
using Adventour.Api.Services.Day;
using Dapper;
using System.Data;

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
                var itineraryDictionary = new Dictionary<int, ItineraryResponse>();

                var result = ExecuteItineraryQuery(itineraryId, itineraryDictionary);

                return result ?? null;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error fetching itinerary: {ex.Message}");
                throw;
            }
        }

        private ItineraryResponse? ExecuteItineraryQuery(int itineraryId, Dictionary<int, ItineraryResponse> itineraryDictionary)
        {
            var dbService = queryServiceBuilder
                .WithStoredProcedure(StoredProcedures.GetItineraryById)
                .WithParameter(StoredProcedures.Parameters.ItineraryId, itineraryId)
                .Build();

            return dbService.Connection.Query<ItineraryResponse, DayResponse, ItineraryResponse>(
                dbService.StoredProcedure,
                (itinerary, day) =>
                {
                    logger.LogInformation($"Mapping ItineraryId={itinerary.Id}, DayId={day?.Id ?? 0}");

                    if (!itineraryDictionary.TryGetValue(itinerary.Id, out var itineraryEntry))
                    {
                        logger.LogInformation($"Creating new itinerary entry for Id={itinerary.Id}");

                        itineraryEntry = new ItineraryResponse
                        {
                            Id = itinerary.Id,
                            Title = itinerary.Title,
                            CreatedAt = itinerary.CreatedAt,
                            Days = new List<DayResponse>()
                        };

                        itineraryDictionary[itinerary.Id] = itineraryEntry;
                    }

                    if (day != null && day.Id != 0)
                    {
                        logger.LogInformation($"Adding DayId={day.Id} to ItineraryId={itinerary.Id}");
                        day.ItineraryId = itinerary.Id;
                        itineraryEntry.Days.Add(day);
                    }
                    else
                    {
                        logger.LogInformation($"No valid day found for ItineraryId={itinerary.Id}");
                    }

                    return itineraryEntry;
                },
                param: dbService.Parameters,
                commandType: CommandType.StoredProcedure,
                splitOn: "Id"
            ).Distinct().FirstOrDefault();
        }



    }
}