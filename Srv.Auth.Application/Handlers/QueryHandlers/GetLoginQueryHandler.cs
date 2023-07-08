using Srv.Auth.Application.IServices;
using Srv.Auth.Domain.Models;
using Srv.Auth.Domain.Queries;
using Srv.Auth.Domain.Responses.CommandResponses;
using MediatR;

namespace Srv.Auth.Application.Handlers.QueryHandlers
{
    public class GetLoginQueryHandler : IRequestHandler<GetLoginQuery, CreateSessionrResponse>
    {
        private readonly ILoginService _loginService;

        public GetLoginQueryHandler(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public async Task<CreateSessionrResponse> Handle(GetLoginQuery request, CancellationToken cancellationToken)
        {
            return await _loginService.LoginAsync(new UserModel { CpfCnpj = request.CpfCnpj, Senha = request.Senha, CodigoAcesso = request.CodigoAcesso });
        }
    }
}
