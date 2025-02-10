using Adventour.Api.Models.Authentication;

namespace Adventour.Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        bool UserExists(string email);
        bool UserExists(Guid userId);
        string CreateUser(UserRegistration registration);
        string AuthenticateUser(UserRegistration registration);
        bool UpdatePublicData(UserUpdate data, Guid userId);

        //string PatchPublicData(UserPatch publicData);
    }
}