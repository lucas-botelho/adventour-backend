using Adventour.Api.Models.Authentication;

namespace Adventour.Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        bool UserExists(string email);
        bool UserExists(Guid userId);
        string CreateUser(UserRegistrationRequest registration);
        string AuthenticateUser(UserRegistrationRequest registration);
        bool UpdatePublicData(UserUpdateRequest data, Guid userId);

        //string PatchPublicData(UserPatch publicData);
    }
}