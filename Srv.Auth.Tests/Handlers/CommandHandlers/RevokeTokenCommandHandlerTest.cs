using Microsoft.Extensions.Configuration;
using Moq;
using Shouldly;
using Gpca.Srv.Auth.Application.Handlers.CommandHandlers;
using Gpca.Srv.Auth.Application.Services;
using Gpca.Srv.Auth.Domain.Responses.CommandResponses;
using Gpca.Srv.Auth.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Gpca.Srv.Auth.Application.IServices;
using Gpca.Srv.Auth.Repository.IRepositories;

namespace Gpca.Srv.Auth.Tests
{
    public class RevokeTokenCommandHandlerTest
    {
        private Dictionary<string, string> appSettingsKeys = new Dictionary<string, string>()
            {
                {"Key1", "Value1"},
                {"Jwt:Issuer", "WebApiJwt.com"},
                {"Jwt:Audience", "localhost"},
                {"Jwt:Key", "39846cc178804fe18610bb1b205cfb16"},
                {"Jwt:AccessTokenExpirationInMinutes", "60"},
                {"Jwt:RefreshTokenExpirationInDays", "7"}
            };

        [Fact]
        public async Task RevokeTokenShouldCallAdd_Method_Once()
        {
            var token = "18a285ee18c446fead1d46a4ae494c8b";

            var configuration = new ConfigurationBuilder().AddInMemoryCollection(appSettingsKeys).Build();

            var options = new DbContextOptionsBuilder<Repository.Contexts.AuthContext>().UseInMemoryDatabase("db_guardian_auth").Options;

            var mockAuthService = new Mock<IAuthService>();

            mockAuthService.Setup(x => x.RevokeJwtToken(It.IsAny<Domain.Commands.RevokeTokenCommand>())).Returns(Task.FromResult(true));

            var handler = new RevokeTokenCommandHandler(mockAuthService.Object);
            var result = await handler.Handle(new Domain.Commands.RevokeTokenCommand() { Token = token }, CancellationToken.None);

            result.ShouldBeOfType<RevokeTokenResponse>();

            mockAuthService.Verify(x => x.RevokeJwtToken(It.IsAny<Domain.Commands.RevokeTokenCommand>()), Times.Once);
        }
    }
}