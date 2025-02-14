using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Adventour.Api.Configurations;
using Adventour.Api.Services.Email.Interfaces;

public class SendGridService : IEmailService
{
    private readonly ILogger<IEmailService> logger;
    private readonly SendGridSettings settings;
    private readonly string apiKey;
    private const string loggerHeader = "## Email Service ##: ";

    public SendGridService(IOptions<SendGridSettings> options, ILogger<SendGridService> logger)
    {
        this.logger = logger;
        this.apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")!;
        this.settings = options.Value;
    }

    public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
    {
        if (string.IsNullOrWhiteSpace(toEmail) || string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(body))
            return false;
        
        try
        {
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(settings.FromEmail, "Adventour App");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, body, body);
            var response = await client.SendEmailAsync(msg);

            return response.StatusCode is System.Net.HttpStatusCode.OK or System.Net.HttpStatusCode.Accepted;
        }
        catch (Exception ex)
        {
            logger.LogError($"{loggerHeader} {ex.Message}");
        }

        return false;
    }
}
