using Adventour.Api.Builders;
using Adventour.Api.Builders.Interfaces;
using Adventour.Api.Infrastructure.Authentication;
using Adventour.Api.Models.Authentication;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace Adventour.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ITokenProvider tokenProvider;
        private readonly IUserRepository userRepository;

        public AuthenticationController(ITokenProvider tokenProvider, IUserRepository userRepository)
        {
            this.tokenProvider = tokenProvider;
            this.userRepository = userRepository;
        }

        [HttpGet(Name = "anonymous-token")]
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

        [HttpPost(Name = "register")]
        //[Authorize]
        public IActionResult Register(UserRegistration user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseApiResponse<ModelStateDictionary>()
                {
                    Data = ModelState,
                    Success = false,
                    Message = "Form model is invalid.",
                });
            }

            if (userRepository.UserExists(user.Email))
            {
                return StatusCode(409, new BaseApiResponse<string>()
                {
                    Data = null,
                    Success = false,
                    Message = "User already exists",
                });
            }

            try
            {
                var userId = userRepository.CreateUser(user);
                //var token = this.tokenProvider.Create(userId);

                return Ok(new BaseApiResponse<string>()
                {
                    Data = userId,
                    Success = true,
                    Message = "Token created successfully",
                });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new BaseApiResponse<string>()
                {
                    Data = null,
                    Success = false,
                    Message = "User creation failed",
                });
            }           
        }

        [HttpPost(Name = "login")]
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

        //[HttpGet]
        //[Authorize]
        ////hello world
        //public IActionResult HelloWorld()
        //{
        //    return Ok(new BaseApiResponse<string>()
        //    {
        //        Data = "Hello World",
        //        Success = true,
        //        Message = "Hello World",
        //    });
        //}
    }
}
