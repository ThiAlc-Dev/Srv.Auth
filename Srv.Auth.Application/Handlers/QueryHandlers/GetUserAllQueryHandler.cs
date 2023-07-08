using Srv.Auth.Application.IServices;
using Srv.Auth.Domain.Commands;
using Srv.Auth.Domain.Responses.CommandResponses;
using MediatR;

namespace Srv.Auth.Application.Handlers.QueryHandlers
{
    public class GetUserAllQueryHandler : IRequestHandler<GetUserAllCommand, DataResponse>
    {
        private readonly ILoginService _loginService;

        public GetUserAllQueryHandler(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public async Task<DataResponse> Handle(GetUserAllCommand request, CancellationToken cancellationToken)
        {
            return await _loginService.GetUserAllAsync();
        }
    }
}
