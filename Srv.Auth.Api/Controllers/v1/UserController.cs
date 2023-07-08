using Srv.Auth.Api.Attributes;
using Srv.Auth.Domain.Commands;
using Srv.Auth.Domain.Responses.CommandResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Srv.Auth.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1")]
    [ValidateSessionToken]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("GetUserAll")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DataResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(DataResponse))]
        public async Task<DataResponse> GetUserAll([FromBody] GetUserAllCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost]
        [Route("EditUser")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DataResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(DataResponse))]
        public async Task<DataResponse> EditUser([FromBody] EditUserCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}
