namespace Adventour.Api.Services.Authentication
{
    public interface ITokenProviderService
    {
        string Create(string userId);
    }
}