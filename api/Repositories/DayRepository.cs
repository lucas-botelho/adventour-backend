using Adventour.Api.Builders.Interfaces;
using Adventour.Api.Constants.Database;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Requests.Day;
using Adventour.Api.Responses.Day;
using System.Data;

namespace Adventour.Api.Repositories
{
    public class DayRepository : IDayRepository
    {
        private readonly IQueryServiceBuilder queryServiceBuilder;
        private readonly ILogger<DayRepository> logger;
        private const string logHeader = "## DayRepository ##: ";

        public DayRepository(IQueryServiceBuilder queryServiceBuilder, ILogger<DayRepository> logger)
        {
            this.queryServiceBuilder = queryServiceBuilder;
            this.logger = logger;
        }

        public List<DayResponse> GetDaysByItineraryId(int itineraryId)
        {
            try
            {
                var dbService = queryServiceBuilder.WithStoredProcedure(StoredProcedures.GetDaysByItineraryId)
                    .WithParameter(StoredProcedures.Parameters.ItineraryId, itineraryId)
                    .Build();

                var days = dbService.QueryMultiple<DayResponse>().ToList();

                return days ?? null;
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
                throw;
            }
        }

        public int AddDay(AddDayRequest request)
        {
            try
            {
                var dbService = queryServiceBuilder.WithStoredProcedure(StoredProcedures.AddDay)
                    .WithParameter(StoredProcedures.Parameters.ItineraryId, request.ItineraryId)
                    .WithParameter(StoredProcedures.Parameters.DayNumber, request.DayNumber)
                    .WithOutputParameter(StoredProcedures.Parameters.InsertedId, DbType.Int32)
                    .Build();

                var insertedId = dbService.InsertSingleWithOutput<int>(StoredProcedures.Parameters.InsertedId);

                return insertedId;
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
                throw;
            }
        }

        public bool DeleteDay(int dayId)
        {
            try
            {
                var dbService = queryServiceBuilder
                    .WithStoredProcedure("DeleteDayById")
                    .WithParameter("@DayId", dayId)
                    .Build();

                return dbService.Delete();
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} Error deleting day {dayId}: {ex.Message}");
                return false;
            }
        }

    }
}
