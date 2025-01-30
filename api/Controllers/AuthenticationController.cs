using api.Infrastructure.Authentication;
using api.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace api.Controllers
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

        [HttpPost(Name = "Token")]
        public IActionResult Token(string username, string password)
        {
            //missing user validation


            var token = this.tokenProvider.Create(username);
            
            return Ok(new ApiResponse<string>()
            {
                Data = token,
                Success = true,
                Message = "Token created successfully",
            });
        }
    }
}
