using Adventour.Api.Builders;
using Adventour.Api.Builders.Interfaces;
using Adventour.Api.Constants.Database;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Requests.TimeSlot;
using Adventour.Api.Responses.TimeSlot;
using Dapper;
using System.Data;

namespace Adventour.Api.Repositories
{
    public class TimeSlotRepository : ITimeSlotRepository
    {
        private readonly IQueryServiceBuilder queryServiceBuilder;
        private readonly ILogger<TimeSlotRepository> logger;
        private const string logHeader = "## TimeSlotRepository ##: ";

        public TimeSlotRepository(IQueryServiceBuilder queryServiceBuilder, ILogger<TimeSlotRepository> logger)
        {
            this.queryServiceBuilder = queryServiceBuilder;
            this.logger = logger;
        }

        public int AddTimeSlot(AddTimeSlotRequest request)
        {
            try
            {
                var dbService = queryServiceBuilder
                    .WithStoredProcedure(StoredProcedures.AddTimeSlot)
                    .WithParameter(StoredProcedures.Parameters.AttractionId, request.AttractionId)
                    .WithParameter(StoredProcedures.Parameters.DayId, request.DayId)
                    .WithParameter(StoredProcedures.Parameters.StartTime, request.StartTime)
                    .WithParameter(StoredProcedures.Parameters.EndTime, request.EndTime)
                    .WithOutputParameter(StoredProcedures.Parameters.InsertedId, DbType.Int32)
                    .Build();

                var insertedId = dbService.InsertSingleWithOutput<int>(StoredProcedures.Parameters.InsertedId);

                return insertedId;
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} Error adding time slot: {ex.Message}");
                throw;
            }
        }

        public void DeleteTimeSlot(int timeSlotId)
        {
            try
            {
                var dbService = queryServiceBuilder
                    .WithStoredProcedure(StoredProcedures.DeleteTimeSlot)
                    .WithParameter(StoredProcedures.Parameters.TimeSlotId, timeSlotId)
                    .Build();

                dbService.Delete();
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} Error deleting time slot: {ex.Message}");
                throw;
            }
        }

        /*public List<TimeSlot> GetTimeSlotsByDayId(int dayId)
        {
            try
            {
                var timeSlotDictionary = new Dictionary<int, TimeSlot>();

                var dbService = queryServiceBuilder
                    .WithStoredProcedure(StoredProcedures.GetTimeSlotsByDayId)
                    .WithParameter(StoredProcedures.Parameters.DayId, dayId)
                    .Build();

                var result = dbService.QueryMultiple<TimeSlot>;

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} Error fetching time slots: {ex.Message}");
                throw;
            }
        }*/

        public TimeSlot GetTimeSlotById(int timeSlotId)
        {
            try
            {
                var dbService = queryServiceBuilder
                    .WithStoredProcedure("GetTimeSlotById")
                    .WithParameter("TimeSlotId", timeSlotId)
                    .Build();

                return dbService.QuerySingle<TimeSlot>();
            }
            catch (Exception ex)
            {
                logger.LogError($"Error fetching time slot by id: {ex.Message}");
                throw;
            }
        }
    }
}
