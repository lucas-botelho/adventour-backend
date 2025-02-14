
namespace Adventour.Api.Services.Authentication
{
    public interface ITokenProviderService
    {
        string Create(string userId);
        string GeneratePinToken(string userId, string pin);
        Task<bool> ValidatePinToken(string token, string enteredPin, string email);
    }
}