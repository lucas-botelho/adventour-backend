using Adventour.Api.Requests.Authentication;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Responses;
using Adventour.Api.Responses.Authentication;
using Adventour.Api.Services.Authentication;
using Adventour.Api.Services.Email.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FirebaseAdmin.Auth;
using Adventour.Api.Data;
using Adventour.Api.Models;


namespace Adventour.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ITokenProviderService tokenProvider;
        private readonly IUserRepository userRepository;
        private readonly ILogger<AuthenticationController> logger;
        private const string logHeader = "## AuthenticationController ##: ";
        private readonly IEmailService emailService;

        public AuthenticationController(ITokenProviderService tokenProvider, IUserRepository userRepository, IEmailService emailService, ILogger<AuthenticationController> logger)
        {
            this.tokenProvider = tokenProvider;
            this.userRepository = userRepository;
            this.logger = logger;
            this.emailService = emailService;
        }

        [HttpGet("exist/{email}")]
        public IActionResult UserByEmail(string email)
        {

            if (userRepository.UserExists(email))
            {
                return StatusCode(409, new BaseApiResponse<EmailRegistredResponse>(new EmailRegistredResponse(true), "User exists"));
            }

            return Ok(new BaseApiResponse<EmailRegistredResponse>(new EmailRegistredResponse(false), "User doesn't exist"));
        }

        [HttpPost("user")]
        [Authorize]
        public async Task<IActionResult> RegisterUser(UserRegistrationRequest user)
        {
            if (userRepository.UserExists(user.Email))
            {
                return StatusCode(409, new BaseApiResponse<string>("User with that email already exists"));
            }

            var userId = userRepository.CreateUser(user).ToString();

            if (!string.IsNullOrEmpty(userId))
            {
                string securityPin = new Random().Next(1000, 9999).ToString();
                var isEmailSent = await this.emailService.SendEmailAsync(user.Email, "Confirmation email", securityPin);
                var token = this.tokenProvider.GeneratePinToken(userId, securityPin);

                return Ok(new BaseApiResponse<TokenResponse>(
                    new TokenResponse() { Token = token, UserId = userId },
                    "User created successfully")
                );
            }

            logger.LogError($"{logHeader} user id is IsNullOrEmpty");
            return StatusCode(500, new BaseApiResponse<string>("Failed to register the user."));
        }

        [HttpPost("email/confirm")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
        {
            string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var isPinValid = await this.tokenProvider.ValidatePinToken(token, request.Pin, request.UserId);

            if (isPinValid)
            {
                this.userRepository.ConfirmEmail(request.UserId);

                return Ok(new BaseApiResponse<TokenResponse>(
                    new TokenResponse()
                    {
                        Token = tokenProvider.Create(request.UserId)
                    },
                    "Email validated successfully")
                );
            }

            return StatusCode(500, new BaseApiResponse<string>("Verification failed. Please check your details and try again."));
        }


        [HttpPatch("user/{userId}")]
        //[Authorize]
        public IActionResult UpdateUser(string userId, [FromBody] UserUpdateRequest data)
        {
            var userIdGuid = new Guid(userId);

            var isUpdated = userRepository.UpdatePublicData(data, userIdGuid);

            return isUpdated ? Ok(new BaseApiResponse<UpdateUserPublicDataResponse>(new UpdateUserPublicDataResponse(isUpdated), "User updated successfully"))
                : StatusCode(404, new BaseApiResponse<string>("User does not exist"));
        }

        [HttpGet("user/me")]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var user = await this.userRepository.GetUser(token);

            return user is null 
                ? NotFound(new BaseApiResponse<string>("User not found."))
                : Ok(new BaseApiResponse<Person>(user, "User found."));
        }
    }
}
