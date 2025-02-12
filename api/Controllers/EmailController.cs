using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Adventour.Api.Services;  // Assuming your EmailService is here

namespace Adventour.Api.Controllers
{
	// Define the route and specify that this controller handles API requests
	[Route("api/[controller]")]
	[ApiController]
	public class EmailController : ControllerBase
	{
		// Declare the email service to send emails
		private readonly IEmailService _emailService;

		// Constructor where the EmailService is injected into the controller
		public EmailController(IEmailService emailService)
		{
			_emailService = emailService;  // Assign the injected service to the private field
		}

		// Endpoint for sending an email
		// POST: api/email/send
		[HttpPost("send")]
		public async Task<IActionResult> SendEmailAsync([FromBody] EmailRequest emailRequest)
		{
			// Step 1: Validate that the email address is in a correct format
			if (!IsValidEmail(emailRequest.ToEmail))
			{
				// If the email is invalid, return a 400 Bad Request response with a message
				return BadRequest("Invalid email address.");
			}

			// Step 2: Validate that the email subject is not empty or just whitespace
			if (string.IsNullOrWhiteSpace(emailRequest.Subject))
			{
				// If the subject is empty, return a 400 Bad Request response
				return BadRequest("Subject cannot be empty.");
			}

			// Step 3: Validate that the email body is not empty or just whitespace
			if (string.IsNullOrWhiteSpace(emailRequest.Body))
			{
				// If the body is empty, return a 400 Bad Request response
				return BadRequest("Body cannot be empty.");
			}

			try
			{
				// Step 4: Try to send the email using the injected email service
				await _emailService.SendEmailAsync(emailRequest.ToEmail, emailRequest.Subject, emailRequest.Body);

				// Step 5: If sending the email succeeds, return a 200 OK response with a success message
				return Ok("Email sent successfully.");
			}
			catch (Exception ex)
			{
				// Step 6: Handle any exceptions that occur during email sending
				// For security reasons, we don't reveal detailed exception info
				// Return a generic 500 Internal Server Error response with a message
				return StatusCode(500, "An error occurred while sending the email.");
			}
		}

		// Helper method to validate if the email is in a valid format
		// This method uses regular expression to ensure the email follows a basic format (e.g., example@example.com)
		private bool IsValidEmail(string email)
		{
			// The regular expression checks that the email has text before and after the '@' and contains a valid domain
			return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
		}
	}

	// Model to represent the request body for sending an email
	// This class holds the data that the client sends when requesting to send an email
	public class EmailRequest
	{
		// The recipient's email address
		public string ToEmail { get; set; }

		// The subject of the email
		public string Subject { get; set; }

		// The body content of the email
		public string Body { get; set; }
	}
}
