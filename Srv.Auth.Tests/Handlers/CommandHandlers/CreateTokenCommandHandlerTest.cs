using Microsoft.Extensions.Configuration;
using Moq;
using Shouldly;
using Gpca.Srv.Auth.Application.Handlers.CommandHandlers;
using Gpca.Srv.Auth.Application.Services;
using Gpca.Srv.Auth.Domain.Responses.CommandResponses;
using Gpca.Srv.Auth.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Gpca.Srv.Auth.Application.IServices;
using Gpca.Srv.Auth.Repository.IRepositories;

namespace Gpca.Srv.Auth.Tests
{
    public class CreateTokenCommandHandlerTest
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
        public async Task CreateTokenShouldCallAdd_Method_Once()
        {
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(appSettingsKeys).Build();

            var options = new DbContextOptionsBuilder<Repository.Contexts.AuthContext>().UseInMemoryDatabase("db_guardian_auth").Options;

            var mockAuthService = new Mock<IAuthService>();

            mockAuthService.Setup(x => x.GenerateJwtToken()).Returns(Task.FromResult(""));
            mockAuthService.Setup(x => x.GetTokenId(It.IsAny<string>())).Returns(Task.FromResult(123));
            mockAuthService.Setup(x => x.GenerateRefreshToken(It.IsAny<int>())).Returns(Task.FromResult(""));

            var handler = new CreateTokenCommandHandler(mockAuthService.Object, configuration);
            var result = await handler.Handle(new Domain.Commands.CreateTokenCommand() { }, CancellationToken.None);

            result.ShouldBeOfType<CreateTokenResponse>();

            mockAuthService.Verify(x => x.GenerateJwtToken(), Times.Once);
            mockAuthService.Verify(x => x.GetTokenId(It.IsAny<string>()), Times.Once);
            mockAuthService.Verify(x => x.GenerateRefreshToken(It.IsAny<int>()), Times.Once);
        }
    }
}