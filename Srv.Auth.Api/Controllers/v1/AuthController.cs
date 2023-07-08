using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Srv.Auth.Domain.Commands;
using Srv.Auth.Domain.Responses.CommandResponses;
using Srv.Auth.Domain.Queries;
using static Srv.Auth.Domain.Queries.GetLoginQuery;
using FluentValidation.Results;
using FluentValidation;
using Srv.Auth.Api.Attributes;

namespace Srv.Auth.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("CreateUser")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ForgotPasswordResponse))]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            var validator = new RegisterValidator();
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

        [HttpPost]
        [Route("RedefintionPassword")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ForgotPasswordResponse))]
        public async Task<IActionResult> RedefintionPassword([FromBody] ForgotPasswordCommand command)
        {
            var validator = new ForgotPassValidator();
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

        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ForgotPasswordResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ForgotPasswordResponse))]
        public async Task<IActionResult> Login([FromBody] GetLoginQuery command)
        {
            var validator = new LoginValidator();
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

        //[HttpPost]
        //[Route("Logout")]
        //[AllowAnonymous]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LogoutUserResponse))]
        //public async Task<LogoutUserResponse> Logout([FromBody] LogoutUserCommand command)
        //{
        //    return await _mediator.Send(command);
        //}

        [HttpPost]
        [Route("ValidateSession")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ValidateSessionResponse))]
        public async Task<ValidateSessionResponse> ValidateSession([FromBody] ValidateSessionCommand command)
        {
            return await _mediator.Send(command);
        }

        
    }
}
