namespace api.Infrastructure.Authentication
{
    public interface ITokenProvider
    {
        string Create(string username);
    }
}