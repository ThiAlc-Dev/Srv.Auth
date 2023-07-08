using Srv.Auth.Application.IServices;
using Srv.Auth.Domain.Commands;
using Srv.Auth.Domain.Responses.CommandResponses;
using MediatR;

namespace Srv.Auth.Application.Handlers.CommandHandlers
{
    public class PasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, ForgotPasswordResponse>
    {
        private readonly ILoginService _loginService;

        public PasswordCommandHandler(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public async Task<ForgotPasswordResponse> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            return await _loginService.RedefinitionPassword(request);
        }
    }
}
