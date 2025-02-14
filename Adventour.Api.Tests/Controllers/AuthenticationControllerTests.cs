using Adventour.Api.Controllers;
using Moq;
using Adventour.Api.Services.Authentication;
using Adventour.Api.Repositories.Interfaces;
using Adventour.Api.Models.Authentication;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Adventour.Api.Responses;

namespace Adventour.Api.Tests.Controllers
{
    public class AuthenticationControllerTests
    {
        [Fact]
        public void Register_ValidUser_ReturnsOk()
        {
            //shoulnd't mock the services here
            //mocking if for when the content of the service is not relevant to the test
            //if the content of the service is relevant to the test, then you should use the real service
            var tokenProviderService = new Mock<ITokenProviderService>();
            tokenProviderService.Setup(x => x.Create(It.IsAny<string>()))
                .Returns("mocked-token");

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(x => x.CreateUser(It.IsAny<UserRegistrationRequest>()))
                .Returns("mocked-user-id");

            var controller = new AuthenticationController(tokenProviderService.Object, userRepository.Object);

            var validUser = new UserRegistrationRequest
            {
                Name = "John Doe",
                Password = "StrongP@ss123",
                ConfirmPassword = "StrongP@ss123",
                Email = "john.doe@example.com"
            };


            //var missingFieldsUser = new UserRegistration
            //{
            //    Name = "", // Required field is empty
            //    Password = "", // Required field is empty
            //    ConfirmPassword = "", // Required field is empty
            //    Email = "" // Required field is empty
            //};

            //var invalidUser = new UserRegistration
            //{
            //    Name = "bo",
            //    Password = "ValidP@ss123",
            //    ConfirmPassword = "Valss123",
            //    Email = "invalid-email" 
            //};


            var result = controller.RegisterUser(validUser);

            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            var value = okResult!.Value as BaseApiResponse<string>;
            value!.Data.Should().Be("mocked-user-id");
        }
    }
}