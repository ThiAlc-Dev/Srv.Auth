using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace Srv.Auth.Domain.Entities
{
    public class User : BaseEntity
    {
        public string CpfCnpj { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }

    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.CpfCnpj).NotEmpty()
                .WithMessage("CPF/CNPJ é obrigatório");
            RuleFor(u => u.Nome).NotEmpty();
            RuleFor(u => u.Email).NotEmpty()
                .WithMessage("Endereço de E-mail é obrigatório")
                .EmailAddress()
                .WithMessage("É obriagtório um E-mail válido");
            RuleFor(u => u.Senha).NotEmpty();
        }
    }
}
