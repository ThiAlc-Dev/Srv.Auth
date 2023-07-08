using Srv.Auth.Domain.Responses.CommandResponses;
using MediatR;

namespace Srv.Auth.Domain.Commands
{
    public class GetUserAllCommand : IRequest<DataResponse>
    {
    }
}
