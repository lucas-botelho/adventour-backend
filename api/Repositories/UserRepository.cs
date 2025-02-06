using Adventour.Api.Builders.Interfaces;
using Adventour.Api.Constants.Database;
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

        public bool UserExists(string username, string email)
        {
            try
            {
                var userExists = queryServiceBuilder.WithStoredProcedure(StoredProcedures.CheckUserExistsByEmailAndUsername)
                .WithParameter("@Username", username)
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
