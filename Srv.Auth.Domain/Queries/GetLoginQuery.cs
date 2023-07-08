using FluentValidation;
using Srv.Auth.Domain.Entities;
using Srv.Auth.Domain.Responses.CommandResponses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Srv.Auth.Domain.Queries
{
    public class GetLoginQuery : IRequest<CreateSessionrResponse>
    {
        public string CpfCnpj { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string CodigoAcesso { get; set; } = string.Empty;

        public class LoginValidator : AbstractValidator<GetLoginQuery>
        {
            public LoginValidator()
            {
                RuleFor(u => u.CpfCnpj).NotEmpty()
                    .WithMessage("O CPF/CNPJ é obrigatório");
                RuleFor(u => u.Senha).NotEmpty()
                    .WithMessage("A senha é obrigatória");
            }
        }
    }
}
