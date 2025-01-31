namespace Adventour.Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        bool UserExists(string username, string email);
    }
}