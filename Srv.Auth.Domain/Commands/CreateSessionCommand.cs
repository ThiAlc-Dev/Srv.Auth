using Srv.Auth.Domain.Responses.CommandResponses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Srv.Auth.Domain.Commands
{
    public class CreateSessionCommand : IRequest<CreateSessionrResponse>
    {
        public string CpfCnpj { get; set; }
    }
}
