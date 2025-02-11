using Adventour.Api.Builders.Interfaces;
using Adventour.Api.Constants.Database;
using Adventour.Api.Models.Authentication;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Services.FileUpload.Interfaces;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

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

        public string AuthenticateUser(UserRegistration registration)
        {
            throw new NotImplementedException();
        }

        public string CreateUser(UserRegistration registration)
        {
            //todo : unit test
            try
            {
                var userId = queryServiceBuilder.WithStoredProcedure(StoredProcedures.CreateUser)
                    .WithParameter(StoredProcedures.Parameters.UserId, Guid.NewGuid())
                    .WithParameter(StoredProcedures.Parameters.Name, registration.Name)
                    .WithParameter(StoredProcedures.Parameters.Password, new PasswordHasher<string>().HashPassword(registration.Email, registration.Password))
                    .WithParameter(StoredProcedures.Parameters.Email, registration.Email)
                    .Execute<string>();


                return userId;
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
                throw;
            }
        }

        public bool UpdatePublicData(UserUpdate data, Guid userId)
        {
            try
            {
                var result = queryServiceBuilder.WithStoredProcedure(StoredProcedures.UpdateUserPublicData)
                    .WithParameter(StoredProcedures.Parameters.UserId, userId.ToString())
                    .WithParameter(StoredProcedures.Parameters.Username, data.UserName)
                    .WithParameter(StoredProcedures.Parameters.ProfilePictureReference, data.ImagePublicId)
                    .Execute<int>();

                return result > 0;
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
                return false;
            }

        }

        public bool UserExists(string email)
        {
            try
            {
                var userExists = queryServiceBuilder.WithStoredProcedure(StoredProcedures.CheckUserExistsByEmail)
                .WithParameter(StoredProcedures.Parameters.Email, email)
                .Execute<int>();

                return userExists > 0;
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
                throw;
            }
        }

        public bool UserExists(Guid userId)
        {
            try
            {
                var userExists = queryServiceBuilder.WithStoredProcedure(StoredProcedures.CheckUserExistsByEmail)
                .WithParameter(StoredProcedures.Parameters.UserId, userId.ToString())
                .Execute<int>();

                return userExists > 0;
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader} {ex.Message}");
                throw;
            }
        }
    }
}
