using DotNetEnv;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Options;
using Adventour.Api.Configurations;

public class EmailService : IEmailService
{
	private readonly SendGridSettings _settings;
	private readonly string _apiKey;

	public EmailService(IOptions<SendGridSettings> options)
	{
		DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
		_settings = options.Value;
		_apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
	}

	public async Task SendEmailAsync(string toEmail, string subject, string body)
	{
		var client = new SendGridClient(_apiKey);
		var from = new EmailAddress(_settings.FromEmail, "Adventour App");
		var to = new EmailAddress(toEmail);
		var msg = MailHelper.CreateSingleEmail(from, to, subject, body, body);
		await client.SendEmailAsync(msg);
	}
}
