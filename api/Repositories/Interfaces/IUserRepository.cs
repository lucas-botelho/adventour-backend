using Adventour.Api.Models;
using Adventour.Api.Requests.Authentication;
using Adventour.Api.Responses.Authentication;

namespace Adventour.Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        bool UserExists(string email);
        bool UserExists(Guid userId);
        Guid CreateUser(UserRegistrationRequest registration);
        string AuthenticateUser(UserRegistrationRequest registration);
        bool UpdatePublicData(UserUpdateRequest data, Guid userId);
        void ConfirmEmail(string userId);
        Task<Person?> GetUser(string token);
    }
}