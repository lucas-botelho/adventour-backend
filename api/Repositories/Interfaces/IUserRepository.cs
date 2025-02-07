using Adventour.Api.Models.Authentication;

namespace Adventour.Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        bool UserExists(string email);

        string CreateUser(UserRegistration registration);
    }
}