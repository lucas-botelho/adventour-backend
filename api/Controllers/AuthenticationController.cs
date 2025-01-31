using Adventour.Api.Builders;
using Adventour.Api.Builders.Interfaces;
using Adventour.Api.Infrastructure.Authentication;
using Adventour.Api.Models.Authentication;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Adventour.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ITokenProvider tokenProvider;
        private readonly IUserRepository userRepository;

        public AuthenticationController(ITokenProvider tokenProvider, IUserRepository userRepository)
        {
            this.tokenProvider = tokenProvider;
            this.userRepository = userRepository;
        }

        [HttpPost(Name = "Register")]
        public IActionResult Register(UserRegistration user)
        {

            if (userRepository.UserExists(user.Username, user.Email))
            {
                //return error message
            }

            

            //if (user.Password != user.ConfirmPassword)
            //{
            //    return BadRequest(new BaseApiResponse<string>()
            //    {
            //        Data = null,
            //        Success = false,
            //        Message = "Passwords do not match",
            //    });
            //}

            //missing user validation
            var token = this.tokenProvider.Create("");

            return Ok(new BaseApiResponse<string>()
            {
                Data = token,
                Success = true,
                Message = "Token created successfully",
            });
        }

        [HttpPost(Name = "Login")]
        public IActionResult Login(User user)
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

        [HttpGet]
        [Authorize]
        //hello world
        public IActionResult HelloWorld()
        {
            return Ok(new BaseApiResponse<string>()
            {
                Data = "Hello World",
                Success = true,
                Message = "Hello World",
            });
        }
    }
}
