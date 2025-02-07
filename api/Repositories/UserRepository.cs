using Adventour.Api.Builders.Interfaces;
using Adventour.Api.Constants.Database;
using Adventour.Api.Models.Authentication;
using Adventour.Api.Repositories.Interfaces;

namespace Adventour.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IQueryServiceBuilder queryServiceBuilder;

        public UserRepository(IQueryServiceBuilder dbConnectionServiceBuilder)
        {
            this.queryServiceBuilder = dbConnectionServiceBuilder;
        }

        public string CreateUser(UserRegistration registration)
        {

            //todo : unit test
            //todo : log
            //return do id gerado
            //throw proper exception
            try
            {
                var result = queryServiceBuilder.WithStoredProcedure(StoredProcedures.CreateUser)
                    .WithParameter("@id_user", Guid.NewGuid().ToString("D"))
                    .WithParameter("@name", registration.Name)
                    .WithParameter("@password", registration.Password)
                    .WithParameter("@email", registration.Email)
                    .Execute<string>();


                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool UserExists(string email)
        {
            try
            {
                var userExists = queryServiceBuilder.WithStoredProcedure(StoredProcedures.CheckUserExistsByEmail)
                .WithParameter("@Email", email)
                .Execute<int>();

                return userExists > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
