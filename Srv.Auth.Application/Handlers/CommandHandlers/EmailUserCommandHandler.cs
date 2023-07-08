using Srv.Auth.CrosCutting.Email;
using Srv.Auth.Domain.Commands;
using Srv.Auth.Domain.Responses.CommandResponses;
using MediatR;

namespace Srv.Auth.Application.Handlers.CommandHandlers
{
    public class EmailUserCommandHandler : IRequestHandler<SendEmailCommand, SendEmailResponse>
    {
        private readonly ISmtpEmailClientService _smtpEmailClient;

        public EmailUserCommandHandler(ISmtpEmailClientService smtpEmailClient)
        {
            _smtpEmailClient = smtpEmailClient;
        }

        public async Task<SendEmailResponse> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            return await _smtpEmailClient.SendEmailAsync(request.Destino, request.Assunto, request.Mensagem);
        }
    }
}
