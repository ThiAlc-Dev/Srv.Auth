using Srv.Auth.Repository.Options;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using Srv.Auth.Domain.Responses.CommandResponses;

namespace Srv.Auth.CrosCutting.Email
{
    public class SmtpEmailClientService : ISmtpEmailClientService
    {
        public EmailSettings _emailSettings { get; }

        public SmtpEmailClientService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task<SendEmailResponse> SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                Execute(email, subject, message).Wait();
                return new SendEmailResponse { Mensagem = "Email envidao com sucesso!" };
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task Execute(string email, string subject, string message)
        {
            try
            {
                string toEmail = email;

                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.UsernameEmail, "Equipe PCA")
                };

                mail.To.Add(new MailAddress(toEmail));
                
                if (!string.IsNullOrEmpty(_emailSettings.CcEmail))
                    mail.CC.Add(new MailAddress(_emailSettings.CcEmail));

                mail.Subject = "LEMBRETE DE SENHA - " + subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                // add anexo
                //mail.Attachments.Add(new Attachment(arquivo));
                //

                using (SmtpClient smtp = new SmtpClient(_emailSettings.PrimaryDomain, _emailSettings.PrimaryPort))
                {
                    smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
