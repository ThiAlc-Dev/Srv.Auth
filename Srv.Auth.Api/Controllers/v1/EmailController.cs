using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Srv.Auth.Domain.Commands;
using Srv.Auth.Domain.Responses.CommandResponses;
using FluentValidation.Results;
using Srv.Auth.Application.Handlers.CommandHandlers;

namespace Srv.Auth.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class EmailController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmailController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("SendEmailForgotPass")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SendEmailResponse))]
        public async Task<IActionResult> SendEmailForgotPass([FromBody] SendEmailCommand command)
        {
            var validator = new SendEmailCommandValidator();
            ValidationResult result = await validator.ValidateAsync(command);

            if (result.IsValid)
            {
                return Ok(await _mediator.Send(command));
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
    }
}
