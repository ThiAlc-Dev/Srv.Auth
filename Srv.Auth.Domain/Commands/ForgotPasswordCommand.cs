using FluentValidation;
using Srv.Auth.Domain.Responses.CommandResponses;
using MediatR;

namespace Srv.Auth.Domain.Commands
{
    public class ForgotPasswordCommand : IRequest<ForgotPasswordResponse>
    {
        public string CpfCnpj { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string SenhaConfirmacao { get; set; } = string.Empty;
    }

    public class ForgotPassValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPassValidator()
        {
            RuleFor(u => u.CpfCnpj).NotEmpty()
                .WithMessage("O CPF/CNPJ é obrigatório")
                .Must(x => x.Length == 11 || x.Length == 14)
                .WithMessage("O CPF/CNPJ deve conter 11 ou 14 numeros");
            RuleFor(u => u.Email).NotEmpty()
                .WithMessage("Endereço de E-mail é obrigatório")
                .EmailAddress()
                .WithMessage("É obriagtório um E-mail válido");
            RuleFor(u => u.Senha).NotEmpty()
                .WithMessage("O campo Senha é obrigatório");
            RuleFor(u => u.Senha)
                .Equal(u => u.SenhaConfirmacao)
                .WithMessage("A senhas não são identicas");
        }
    }
}
