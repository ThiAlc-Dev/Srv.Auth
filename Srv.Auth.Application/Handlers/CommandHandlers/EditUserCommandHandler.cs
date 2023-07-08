using Srv.Auth.Application.IServices;
using Srv.Auth.Domain.Commands;
using Srv.Auth.Domain.Responses.CommandResponses;
using MediatR;

namespace Srv.Auth.Application.Handlers.CommandHandlers
{
    public class EditUserCommandHandler : IRequestHandler<EditUserCommand, DataResponse>
    {
        private readonly ILoginService _loginService;

        public EditUserCommandHandler(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public async Task<DataResponse> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            return await _loginService.EditUserAsync(request);
        }
    }
}
