using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Adventour.Api.Configurations;
using Adventour.Api.Services.Email.Interfaces;

public class EmailService : IEmailService
{
    private readonly SendGridSettings _settings;
    private readonly string _apiKey;

    public EmailService(IOptions<SendGridSettings> options)
    {
        _settings = options.Value ?? throw new ArgumentNullException(nameof(options), "SendGrid settings cannot be null.");
        _apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");

        if (string.IsNullOrWhiteSpace(_apiKey))
        {
            throw new InvalidOperationException("SendGrid API key is missing from environment variables.");
        }
    }

    public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
    {
        if (string.IsNullOrWhiteSpace(toEmail))
            throw new ArgumentException("Recipient email cannot be empty.", nameof(toEmail));

        if (string.IsNullOrWhiteSpace(subject))
            throw new ArgumentException("Email subject cannot be empty.", nameof(subject));

        if (string.IsNullOrWhiteSpace(body))
            throw new ArgumentException("Email body cannot be empty.", nameof(body));

        try
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_settings.FromEmail, "Adventour App");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, body, body);
            var response = await client.SendEmailAsync(msg);

            // Return true if email was accepted by SendGrid
            return response.StatusCode is System.Net.HttpStatusCode.OK or System.Net.HttpStatusCode.Accepted;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
            return false;
        }
    }
}
