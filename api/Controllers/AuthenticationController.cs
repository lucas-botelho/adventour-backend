using Adventour.Api.Builders;
using Adventour.Api.Builders.Interfaces;
using Adventour.Api.Models.Authentication;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Responses;
using Adventour.Api.Responses.Authentication;
using Adventour.Api.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;


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

        public AuthenticationController(ITokenProviderService tokenProvider, IUserRepository userRepository, ILogger<AuthenticationController> logger)
        {
            this.tokenProvider = tokenProvider;
            this.userRepository = userRepository;
            this.logger = logger;
        }

        [HttpGet("anonymous-token")]
        public IActionResult AnonymousToken()
        {
            var token = this.tokenProvider.Create(string.Empty);

            if (string.IsNullOrEmpty(token))
            {
                return StatusCode(500, new BaseApiResponse<object>()
                {
                    Data = null,
                    Success = false,
                    Message = "Token creation failed",
                });
            }

            return Ok(new BaseApiResponse<string>()
            {
                Data = token,
                Success = true,
                Message = "Token created successfully",
            });
        }

        [HttpPost("user")]
        //[Authorize]
        public IActionResult RegisterUser(UserRegistration user)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(new BaseApiResponse<ModelStateDictionary>()
            //    {
            //        Data = ModelState,
            //        Success = false,
            //        Message = "Form model is invalid.",
            //    });
            //}

            if (userRepository.UserExists(user.Email))
            {
                return StatusCode(409, new BaseApiResponse<string>()
                {
                    Data = null,
                    Success = false,
                    Message = "User already exists",
                });
            }

            var userId = userRepository.CreateUser(user);

            if (string.IsNullOrEmpty(userId))
            {
                logger.LogError($"{logHeader} retrieved invalid user id");
                return StatusCode(500, new BaseApiResponse<string>()
                {
                    Data = null,
                    Success = false,
                    Message = "User creation failed",
                });
            }

            return Ok(new BaseApiResponse<RegisterUserResponse>()
            {
                Data = new RegisterUserResponse() { UserId = userId },
                Success = true,
                Message = "User created successfully",
            });
        }

        [HttpPatch("user/{userId}")]
        //[Authorize]
        public IActionResult UpdateUser(string userId, [FromBody] UserUpdate data)
        {
            var userIdGuid = new Guid(userId);

            if (userRepository.UserExists(userIdGuid))
            {
                var isUpdated = userRepository.UpdatePublicData(data, userIdGuid);

                if (isUpdated)
                {
                    return Ok(new BaseApiResponse<string>()
                    {
                        Data = userId,
                        Success = true,
                        Message = "User updated successfully",
                    });
                }

                return StatusCode(500, new BaseApiResponse<string>()
                {
                    Data = null,
                    Success = false,
                    Message = "User update failed",
                });
            }

            return StatusCode(404, new BaseApiResponse<string>()
            {
                Data = null,
                Success = false,
                Message = "User does not exist",
            });
        }

        [HttpPost("login")]
        public IActionResult Login(UserRegistration user)
        {
            //missing user validation
            var token = this.tokenProvider.Create("");

            return Ok(new BaseApiResponse<string>()
            {
                Data = token,
                Success = true,
                Message = "Token created successfully",
            });
        }

        [HttpGet("test")]
        public IActionResult test()
        {
            return Ok(new BaseApiResponse<string>()
            {
                Data = "test",
                Success = true,
                Message = "Test",
            });
        }
    }
}
