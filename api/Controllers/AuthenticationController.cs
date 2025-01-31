using Adventour.Api.Infrastructure.Authentication;
using Adventour.Api.Models;
using Adventour.Api.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Adventour.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ITokenProvider tokenProvider;

       

        public AuthenticationController(ITokenProvider tokenProvider)
        {
            this.tokenProvider = tokenProvider;
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
