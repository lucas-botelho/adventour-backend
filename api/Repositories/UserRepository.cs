using Adventour.Api.Builders.Interfaces;
using Adventour.Api.Constants.Database;
using Adventour.Api.Requests.Authentication;
using Adventour.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Adventour.Api.Responses.Authentication;
using FirebaseAdmin.Auth;
using Newtonsoft.Json.Linq;
using Microsoft.IdentityModel.Tokens;
using Adventour.Api.Data;
using Adventour.Api.Models;

namespace Adventour.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ILogger<UserRepository> logger;
        private const string logHeader = "## UserRepository ##: ";
        private readonly AdventourContext db;


        public UserRepository(ILogger<UserRepository> logger, AdventourContext db)
        {
            this.logger = logger;
            this.db = db;
        }

        public string AuthenticateUser(UserRegistrationRequest registration)
        {
            throw new NotImplementedException();
        }

        public Guid CreateUser(UserRegistrationRequest registration)
        {
            var user = new Person
            {
                Name = registration.Name,
                Email = registration.Email,
                OauthId = registration.OAuthId,
                PhotoUrl = registration.PhotoUrl
            };

            db.Add(user);
            db.SaveChanges();

            return user.Id;
        }

        public bool UpdatePublicData(UserUpdateRequest data, Guid userId)
        {
            var person = db.Person.FirstOrDefault(p => p.Id == userId);

            if (person == null)
            {
                logger.LogError($"{logHeader} tried updating public data for non existing userId {userId}");
                return false;
            }
            person.Username = data.UserName;
            person.PhotoUrl = data.PublicUrl;
            db.SaveChanges();

            return true;
        }

        public bool UserExists(string email)
        {
            return db.Person.Any(p => p.Email == email);
        }

        public bool UserExists(Guid userId)
        {
            return db.Person.Any(p => p.Id == userId);
        }

        public void ConfirmEmail(string userId)
        {
            Person? person;

            try
            {
                person = db.Person.FirstOrDefault(p => p.Id == new Guid(userId));
            }
            catch (Exception ex)
            {
                logger.LogError($"{logHeader}: {ex.Message}");
                return;
            }

            if (person == null)
            {
                logger.LogError($"{logHeader} userId doesn't exist in database.");
                return;
            }

            person.Verified = true;
            db.SaveChanges();
        }

        public async Task<Person?> GetUser(string token)
        {
            if (token.IsNullOrEmpty())
                return null;

            var decodedToken = await FirebaseAuth.DefaultInstance?.VerifyIdTokenAsync(token) ?? null;

            if (decodedToken is null)
                return null;

            return db.Person.FirstOrDefault(p => p.OauthId == decodedToken.Uid);
        }
    }
}
