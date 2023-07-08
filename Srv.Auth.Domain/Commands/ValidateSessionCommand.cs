using Srv.Auth.Domain.Responses.CommandResponses;
using MediatR;

namespace Srv.Auth.Domain.Commands
{
    public class ValidateSessionCommand : IRequest<ValidateSessionResponse>
    {
        public string RefreshToken { get; set; }
    }
}
