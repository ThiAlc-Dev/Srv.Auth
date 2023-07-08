using Srv.Auth.Domain.Responses.CommandResponses;

namespace Srv.Auth.CrosCutting.Email
{
    public interface ISmtpEmailClientService
    {
        Task<SendEmailResponse> SendEmailAsync(string email, string subject, string message);
    }
}
