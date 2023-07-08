using Srv.Auth.Domain.Responses.CommandResponses;
using MediatR;

namespace Srv.Auth.Domain.Commands
{
    public class EditUserCommand : IRequest<DataResponse>
    {
        public string CpfCnpj { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
