using Srv.Auth.Application.IServices;
using Srv.Auth.Domain.Commands;
using Srv.Auth.Domain.Responses.CommandResponses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Srv.Auth.Application.Handlers.CommandHandlers
{
    public class ValidateSessionCommandHandler : IRequestHandler<ValidateSessionCommand, ValidateSessionResponse>
    {
        private readonly ILoginService _loginService;

        public ValidateSessionCommandHandler(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public async Task<ValidateSessionResponse> Handle(ValidateSessionCommand request, CancellationToken cancellationToken)
        {
            return await _loginService.GetSessionAsync(request);
        }
    }
}
