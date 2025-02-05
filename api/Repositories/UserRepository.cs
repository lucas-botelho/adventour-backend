using Adventour.Api.Builders.Interfaces;
using Adventour.Api.Models.Authentication;
using Adventour.Api.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

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

                var userExists = queryServiceBuilder.WithStoredProcedure("CheckUserExistsByEmailAndUsername")
                .WithParameter("@Username", username)
                .WithParameter("@Email", email)
                .Execute<int>();

                var x = 2;

                return userExists > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
