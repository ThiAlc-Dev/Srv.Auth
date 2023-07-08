using FluentValidation;
using Srv.Auth.Domain.Responses.CommandResponses;
using MediatR;

namespace Srv.Auth.Domain.Commands
{
    public class SendEmailCommand : IRequest<SendEmailResponse>
    {
        public string? Destino { get; set; }
        public string? Assunto { get; set; }
        public string? Mensagem { get; set; }
    }

    public class SendEmailCommandValidator : AbstractValidator<SendEmailCommand>
    {
        public SendEmailCommandValidator()
        {
            RuleFor(x => x.Destino).NotEmpty();
            RuleFor(x => x.Assunto).NotEmpty();
            RuleFor(x => x.Mensagem).NotEmpty();
        }
    }
}