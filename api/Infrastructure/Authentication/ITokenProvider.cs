namespace Adventour.Api.Infrastructure.Authentication
{
    public interface ITokenProvider
    {
        string Create(string userId);
    }
}