using Adventour.Api.Builders.Interfaces;
using Adventour.Api.Constants.Database;
using Adventour.Api.Requests.Authentication;
using Adventour.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Adventour.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IQueryServiceBuilder queryServiceBuilder;
        private readonly ILogger<UserRepository> logger;
        private const string logHeader = "## UserRepository ##: ";

        public UserRepository(IQueryServiceBuilder dbConnectionServiceBuilder, ILogger<UserRepository> logger)
        {
            this.queryServiceBuilder = dbConnectionServiceBuilder;
            this.logger = logger;
        }

        public string AuthenticateUser(UserRegistrationRequest registration)
        {
            throw new NotImplementedException();
        }

        public Guid CreateUser(UserRegistrationRequest registration)
        {
            //todo : unit test

            var dbService = queryServiceBuilder.WithStoredProcedure(StoredProcedures.CreateUser)
                .WithParameter(StoredProcedures.Parameters.Name, registration.Name)
                .WithParameter(StoredProcedures.Parameters.OAuthId, registration.OAuthId)
                .WithParameter(StoredProcedures.Parameters.Email, registration.Email)
                .WithParameter(StoredProcedures.Parameters.PhotoUrl, registration.PhotoUrl)
                .Build();


            return dbService.QuerySingle<Guid>();
        }

        public bool UpdatePublicData(UserUpdateRequest data, Guid userId)
        {
            var dbService = queryServiceBuilder.WithStoredProcedure(StoredProcedures.UpdateUserPublicData)
                .WithParameter(StoredProcedures.Parameters.UserId, userId)
                .WithParameter(StoredProcedures.Parameters.Username, data.UserName)
                .WithParameter(StoredProcedures.Parameters.ProfilePictureReference, data.PublicUrl)
                .Build();

            return dbService.Update();
        }

        public bool UserExists(string email)
        {
            var dbService = queryServiceBuilder.WithStoredProcedure(StoredProcedures.CheckUserExistsByEmail)
            .WithParameter(StoredProcedures.Parameters.Email, email)
            .Build();

            return dbService.QuerySingle<int>() > 0;
        }

        public bool UserExists(Guid userId)
        {
            var dbService = queryServiceBuilder.WithStoredProcedure(StoredProcedures.CheckUserExistsById)
            .WithParameter(StoredProcedures.Parameters.UserId, userId)
            .Build();

            return dbService.QuerySingle<int>() > 0;
        }

        public void ConfirmEmail(string userId)
        {
            var dbService = queryServiceBuilder.WithStoredProcedure(StoredProcedures.ConfirmEmail)
                .WithParameter(StoredProcedures.Parameters.UserId, userId)
                .Build();

            dbService.QuerySingle<int>();
        }
    }
}
