using Srv.Auth.Application.IServices;
using Srv.Auth.Domain.Commands;
using Srv.Auth.Domain.Responses.CommandResponses;
using MediatR;

namespace Srv.Auth.Application.Handlers.CommandHandlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ForgotPasswordResponse>
    {
        private readonly IAuthService _authService;

        public CreateUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<ForgotPasswordResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            return await _authService.CreateUserAsync(request);
        }
    }
}
