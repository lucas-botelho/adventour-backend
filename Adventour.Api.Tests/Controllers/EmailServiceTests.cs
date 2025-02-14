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
    // Test class for EmailService
    public class EmailServiceTests
    {
        private readonly Mock<IOptions<SendGridSettings>> _mockOptions;
        private readonly EmailService _emailService;

        public EmailServiceTests()
        {
            // Create mock options with test settings
            _mockOptions = new Mock<IOptions<SendGridSettings>>();
            _mockOptions.Setup(x => x.Value).Returns(new SendGridSettings { FromEmail = "adventour.helpcenter@gmail.com" });

            // Set up test API key in environment variables
            Environment.SetEnvironmentVariable("SENDGRID_API_KEY", "test-api-key");
            _emailService = new EmailService(_mockOptions.Object);
        }

        // Test successful email sending
        [Fact]
        public async Task SendEmailAsync_WithValidInputs_ReturnsTrue()
        {
            // Arrange
            var toEmail = "a22204044@alunos.ulht.pt";
            var subject = "Test Subject";
            var body = "Test Body";

            // Act
            var result = await _emailService.SendEmailAsync(toEmail, subject, body);

            // Assert
            Assert.True(result);
        }

        // Test null input validation
        [Theory]
        [InlineData(null, "Subject", "Body")] // Test with null email
        [InlineData("a22204044@alunos.ulht.pt", null, "Body")] // Test with null subject
        [InlineData("a22204044@alunos.ulht.pt", "Subject", null)] // Test with null body
        public async Task SendEmailAsync_WithNullInputs_ThrowsArgumentException(string email, string subject, string body)
        {
            // Act & Assert - verify that the method throws an ArgumentException
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _emailService.SendEmailAsync(email, subject, body));
        }
    }

}