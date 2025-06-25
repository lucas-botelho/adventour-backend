using Adventour.Api.Data;
using Adventour.Api.Models.Database;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Requests.Authentication;
using Adventour.Api.Services.Encryption;
using FirebaseAdmin.Auth;
using Microsoft.IdentityModel.Tokens;

namespace Adventour.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ILogger<UserRepository> logger;
        private readonly AdventourContext db;
        private readonly IEncryptionService encryptionService;
        private const string logHeader = "## UserRepository ##: ";

        public UserRepository(
            ILogger<UserRepository> logger,
            AdventourContext db,
            IEncryptionService encryptionService)
        {
            this.logger = logger;
            this.db = db;
            this.encryptionService = encryptionService;
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
                PhotoUrl = registration.PhotoUrl,
                RegistrationStep = 1,
            };

            db.Add(user);
            db.SaveChanges();

            logger.LogError("Registration Step: " + db.Person.FirstOrDefault(p => p.OauthId == registration.OAuthId).RegistrationStep);

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
            person.RegistrationStep = 3;

            db.SaveChanges();

            logger.LogError("Registration Step: " + db.Person.FirstOrDefault(p => p.Id == userId).RegistrationStep);

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
            person.RegistrationStep = 2;
            db.SaveChanges();

            logger.LogError("Registration Step: " + db.Person.FirstOrDefault(p => p.Id.ToString() == userId).RegistrationStep);
        }

        public async Task<Person?> GetUser(string authHeader)
        {
            if (authHeader.IsNullOrEmpty())
                return null;

            var token = authHeader.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
            var decodedToken = await FirebaseAuth.DefaultInstance?.VerifyIdTokenAsync(token) ?? null;

            if (decodedToken is null)
                return null;

            var person = db.Person.FirstOrDefault(p => p.OauthId == decodedToken.Uid);

            if (person is null)
            {
                logger.LogError($"{logHeader} user with OauthId {decodedToken.Uid} not found in database.");
                return null;
            }

            return person;
        }
    }
}
