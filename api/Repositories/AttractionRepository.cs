using Adventour.Api.Builders.Interfaces;
using Adventour.Api.Constants.Database;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Requests.Attraction;
using Adventour.Api.Responses.Attraction;
using Adventour.Api.Responses.City;
using Dapper;
using SendGrid.Helpers.Errors.Model;
using System.Data;

namespace Adventour.Api.Repositories
{
    public class AttractionRepository : IAttractionRepository
    {
        private readonly IQueryServiceBuilder queryServiceBuilder;
        private readonly ILogger<AttractionRepository> logger;
        private const string logHeader = "## AttractionRepository ##: ";

        public AttractionRepository(IQueryServiceBuilder queryServiceBuilder, ILogger<AttractionRepository> logger)
        {
            this.queryServiceBuilder = queryServiceBuilder;
            this.logger = logger;
        }

        public Attraction GetAttractionById(int attractionId)
        {
            try
            {
                var dbService = queryServiceBuilder
                    .WithStoredProcedure("GetAttractionById")
                    .WithParameter("AttractionId", attractionId)
                    .Build();

                return dbService.Connection.QueryFirstOrDefault<Attraction>(
                    dbService.StoredProcedure,
                    param: dbService.Parameters,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} Error fetching attraction: {ex.Message}");
                throw;
            }
        }

        public int AddAttraction(AddAttractionRequest request)
        {
            try
            {
                var city = GetCityById(request.CityId);

                if (city == null)
                {
                    throw new NotFoundException("That city doesn't exist");
                }

                var dbService = queryServiceBuilder
                .WithStoredProcedure(StoredProcedures.AddAttraction)
                .WithParameter(StoredProcedures.Parameters.CityId, request.CityId)
                .WithParameter(StoredProcedures.Parameters.Name, request.Name)
                .WithParameter(StoredProcedures.Parameters.Description, request.Description)
                .WithParameter(StoredProcedures.Parameters.AddressOne, request.AddressOne)
                .WithParameter(StoredProcedures.Parameters.AddressTwo, request.AddressTwo)
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

        public City GetCityById(int CityId)
        {
            try
            {
                var dbService = queryServiceBuilder
                .WithStoredProcedure(StoredProcedures.GetCityById)
                .WithParameter(StoredProcedures.Parameters.CityId, CityId)
                .Build();

                var city = dbService.QuerySingle<City>();

                return city;
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
                throw;
            }
        }
    }
}
