using System.Threading.Tasks;

namespace Adventour.Api.Services.Email.Interfaces
{
	public interface IEmailService
	{
		Task<bool> SendEmailAsync(string toEmail, string subject, string body);
	}
}
