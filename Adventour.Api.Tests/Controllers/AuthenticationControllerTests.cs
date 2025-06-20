using Adventour.Api.Controllers;
using Adventour.Api.Requests.Authentication;
using Adventour.Api.Responses;
using Adventour.Api.Responses.Authentication;
using Adventour.Api.Services.Authentication;
using Adventour.Api.Services.Email.Interfaces;
using Adventour.Api.Repositories.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace Adventour.Api.Tests.Controllers;

public class AuthenticationControllerTests
{
    private readonly Mock<ITokenProviderService> _tokenProviderMock = new();
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly Mock<IEmailService> _emailServiceMock = new();
    private readonly Mock<ILogger<AuthenticationController>> _loggerMock = new();
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock = new();
    private readonly Mock<IConfiguration> _configMock = new();

    private AuthenticationController _controller;

    public AuthenticationControllerTests()
    {
        _controller = new AuthenticationController(
            _httpClientFactoryMock.Object,
            _configMock.Object,
            _tokenProviderMock.Object,
            _userRepoMock.Object,
            _emailServiceMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task RegisterUser_ValidOAuthUser_ReturnsOkWithToken()
    {
        var request = new UserRegistrationRequest
        {
            Name = "João Silva",
            Email = "joao.silva@example.com",
            OAuthId = "firebase-oauth-id-123",
            PhotoUrl = "https://example.com/photo.jpg"
        };

        var generatedUserId = Guid.NewGuid();
        var pinToken = "123456789";

        _userRepoMock.Setup(r => r.UserExists(request.Email)).Returns(false);
        _userRepoMock.Setup(r => r.CreateUser(request)).Returns(generatedUserId);
        _emailServiceMock.Setup(e => e.SendEmailAsync(request.Email, It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
        _tokenProviderMock.Setup(t => t.GeneratePinToken(generatedUserId.ToString(), It.IsAny<string>())).Returns(pinToken);

        var result = await _controller.RegisterUser(request);

        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;

        var response = okResult!.Value.Should().BeOfType<BaseApiResponse<TokenResponse>>().Subject;
        response.Message.Should().Be("User created successfully");
        response.Data.UserId.Should().Be(generatedUserId.ToString());
        response.Data.Token.Should().Be(pinToken);
    }

    [Fact]
    public async Task RegisterUser_EmailAlreadyExists_ReturnsConflict()
    {
        var request = new UserRegistrationRequest
        {
            Name = "João Silva",
            Email = "joao.silva@example.com",
            OAuthId = "firebase-oauth-id-123",
            PhotoUrl = "https://example.com/photo.jpg"
        };

        _userRepoMock.Setup(r => r.UserExists(request.Email)).Returns(true);

        var result = await _controller.RegisterUser(request);

        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult!.StatusCode.Should().Be(409);

        var response = objectResult.Value.Should().BeOfType<BaseApiResponse<string>>().Subject;
        response.Message.Should().Be("User with that email already exists");
    }

    [Fact]
    public async Task RegisterUser_InvalidEmailFormat_ReturnsBadRequest()
    {
        // Arrange
        var request = new UserRegistrationRequest
        {
            Name = "João Silva",
            Email = "joao.silva@",
            OAuthId = "firebase-oauth-id-123",
            PhotoUrl = "https://example.com/photo.jpg"
        };

        _controller.ModelState.AddModelError("Email", "Email inválido");

        var result = await _controller.RegisterUser(request);

        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequest = result as BadRequestObjectResult;

        var response = badRequest!.Value.Should().BeOfType<BaseApiResponse<string>>().Subject;
        response.Message.Should().Be("Email inválido");
    }

    [Fact]
    public async Task ConfirmEmail_ValidPin_ReturnsAuthToken()
    {
        var pinToken = "pin-token-123";
        var userId = "123";
        var pin = "123456";
        var finalToken = "auth-token";

        var request = new ConfirmEmailRequest
        {
            UserId = userId,
            Pin = pin
        };

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["Authorization"] = $"Bearer {pinToken}";
        _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

        _tokenProviderMock.Setup(p => p.ValidatePinToken(pinToken, pin, userId))
                          .ReturnsAsync(true);

        _tokenProviderMock.Setup(p => p.Create(userId))
                          .Returns(finalToken);

        var result = await _controller.ConfirmEmail(request);

        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;

        var response = okResult!.Value.Should().BeOfType<BaseApiResponse<TokenResponse>>().Subject;
        response.Message.Should().Be("Email validated successfully");
        response.Data.Token.Should().Be(finalToken);

        _userRepoMock.Verify(r => r.ConfirmEmail(userId), Times.Once);
    }

}
