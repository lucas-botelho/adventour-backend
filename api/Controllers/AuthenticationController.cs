using Adventour.Api.Models.Authentication;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Responses;
using Adventour.Api.Responses.Authentication;
using Adventour.Api.Services.Authentication;
using Adventour.Api.Services.Email.Interfaces;
using Microsoft.AspNetCore.Mvc;


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

        [HttpPost("user")]
        //[Authorize]
        public async Task<IActionResult> RegisterUser(UserRegistrationRequest user)
        {
            //if(userRepository.UserExists(user.Email))
            //{
            //    return StatusCode(409, new BaseApiResponse<string>("User already exists"));
            //}

            var userId = userRepository.CreateUser(user);

            if (!string.IsNullOrEmpty(userId))
            {
                string securityPin = new Random().Next(1000, 9999).ToString();
                var isEmailSent = await this.emailService.SendEmailAsync(user.Email, "Confirmation email", securityPin);
                var token = this.tokenProvider.GeneratePinToken(user.Email, securityPin);

                return Ok(new BaseApiResponse<AuthenticationTokenResponse>(
                    new AuthenticationTokenResponse() { Token = token },
                    "User created successfully")
                );
            }

            logger.LogError($"{logHeader} user id is IsNullOrEmpty");
            return StatusCode(500, new BaseApiResponse<string>("User creation failed"));
        }

        [HttpPost("email/validate")]
        public async Task<IActionResult> ValidateEmail([FromBody] ValidateEmailRequest request)
        {
            string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var isPinValid = await this.tokenProvider.ValidatePinToken(token, request.Pin, request.Email);

            if (isPinValid)
            {
                return Ok(new BaseApiResponse<AuthenticationTokenResponse>(
                    new AuthenticationTokenResponse() 
                    { 
                        Token = tokenProvider.Create(request.Email)
                    },
                    "Email validated successfully")
                );
            }

            return StatusCode(500, new BaseApiResponse<string>("Email validation failed"));
        }


        [HttpPatch("user/{userId}")]
        //[Authorize]
        public IActionResult UpdateUser(string userId, [FromBody] UserUpdateRequest data)
        {
            var userIdGuid = new Guid(userId);

            if (userRepository.UserExists(userIdGuid))
            {
                var isUpdated = userRepository.UpdatePublicData(data, userIdGuid);

                if (isUpdated)
                {
                    return Ok(new BaseApiResponse<string>(userId, "User updated successfully"));
                }

                return StatusCode(500, new BaseApiResponse<string>("User update failed"));
            }

            return StatusCode(404, new BaseApiResponse<string>("User does not exist"));
        }
    }
}
