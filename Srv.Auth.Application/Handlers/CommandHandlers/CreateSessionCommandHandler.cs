using Srv.Auth.Application.IServices;
using Srv.Auth.Domain.Commands;
using Srv.Auth.Domain.Responses.CommandResponses;
using MediatR;

namespace Srv.Auth.Application.Handlers.CommandHandlers
{
    public class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand, CreateSessionrResponse>
    {
        private readonly ILoginService _loginService;

        public CreateSessionCommandHandler(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public async Task<CreateSessionrResponse> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
        {
            return await _loginService.CreateSessionAsync(request);
        }
    }
}
