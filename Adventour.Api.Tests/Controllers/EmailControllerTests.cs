using Xunit;                    // Framework for unit testing
using Moq;                      // Library for creating mock objects
using Microsoft.AspNetCore.Mvc; // Required for working with API controllers
using Microsoft.Extensions.Options;
using Adventour.Api.Controllers;
using Adventour.Api.Services.Email.Interfaces;
using Adventour.Api.Configurations;
using Adventour.Api.Repositories.Interfaces;

namespace Adventour.Api.Tests
{
	// Test class for EmailController
	public class EmailControllerTests
	{
		// Mock object for email service - allows us to simulate the email service without actually sending emails
		private readonly Mock<IEmailService> _mockEmailService;
		// Instance of the controller we're testing
		private readonly EmailController _controller;

		// Constructor - runs before each test
		public EmailControllerTests()
		{
			// Create a new mock email service
			_mockEmailService = new Mock<IEmailService>();
			// Create a new controller instance with the mock service
			_controller = new EmailController(_mockEmailService.Object);
		}

		// Test method for successful email sending
		[Fact] // This attribute marks this method as a test
		public async Task SendEmail_WithValidRequest_ReturnsOkResult()
		{
			// Arrange - set up the test conditions
			var request = new EmailRequest
			{
				ToEmail = "test@example.com",
				Subject = "Test Subject",
				Body = "Test Body"
			};
			// Setup the mock to return true when SendEmailAsync is called with any parameters
			_mockEmailService.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(true);

			// Act - perform the action we're testing
			var result = await _controller.SendEmailAsync(request);

			// Assert - verify the results
			var okResult = Assert.IsType<OkObjectResult>(result); // Verify we got an OK (200) result
			Assert.Equal("Email sent successfully.", okResult.Value); // Verify the success message
		}

		// Test method for invalid email address
		[Fact]
		public async Task SendEmail_WithInvalidEmail_ReturnsBadRequest()
		{
			// Arrange - create a request with an invalid email
			var request = new EmailRequest
			{
				ToEmail = "invalid-email", // Invalid email format
				Subject = "Test Subject",
				Body = "Test Body"
			};

			// Act - call the controller method
			var result = await _controller.SendEmailAsync(request);

			// Assert - verify we get a BadRequest result
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			Assert.Equal("Invalid email address.", badRequestResult.Value);
		}

		// Test method for empty fields
		// [Theory] allows us to test multiple scenarios with different input data
		[Theory]
		[InlineData("", "Subject", "Body")] // Test with empty email
		[InlineData("test@example.com", "", "Body")] // Test with empty subject
		[InlineData("test@example.com", "Subject", "")] // Test with empty body
		public async Task SendEmail_WithEmptyFields_ReturnsBadRequest(string email, string subject, string body)
		{
			// Arrange - create request with the test data
			var request = new EmailRequest
			{
				ToEmail = email,
				Subject = subject,
				Body = body
			};

			// Act - call the controller method
			var result = await _controller.SendEmailAsync(request);

			// Assert - verify we get a BadRequest result
			Assert.IsType<BadRequestObjectResult>(result);
		}

		// Test method for when the email service fails
		[Fact]
		public async Task SendEmail_WhenServiceFails_ReturnsInternalServerError()
		{
			// Arrange
			var request = new EmailRequest
			{
				ToEmail = "test@example.com",
				Subject = "Test Subject",
				Body = "Test Body"
			};
			// Setup the mock to throw an exception when called
			_mockEmailService.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.ThrowsAsync(new Exception("Test exception"));

			// Act
			var result = await _controller.SendEmailAsync(request);

			// Assert - verify we get a 500 Internal Server Error
			var statusCodeResult = Assert.IsType<ObjectResult>(result);
			Assert.Equal(500, statusCodeResult.StatusCode);
		}
	}
}