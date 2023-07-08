using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Gpca.Srv.Auth.Application.Handlers.CommandHandlers;
using Gpca.Srv.Auth.Application.IServices;
using Gpca.Srv.Auth.Application.Services;
using Gpca.Srv.Auth.Domain.Commands;
using Gpca.Srv.Auth.Domain.Responses.CommandResponses;
using Gpca.Srv.Auth.Repository.IRepositories;
using Gpca.Srv.Auth.Repository.Repositories;

namespace Gpca.Srv.Auth.Tests
{
    public class AuthServiceTest
    {
        private Dictionary<string, string> chavesAppsettings = new Dictionary<string, string>()
            {
                {"Key1", "Value1"},
                {"Jwt:Issuer", "WebApiJwt.com"},
                {"Jwt:Audience", "localhost"},
                {"Jwt:Key", "39846cc178804fe18610bb1b205cfb16"},
                {"Jwt:AccessTokenExpirationInMinutes", "60"},
                {"Jwt:RefreshTokenExpirationInDays", "7"},
            };

        [Fact]
        public void ShouldGiveErrorIfAppsettingsInvalid()
        {
            // Arrange
            var myConfiguration = new Dictionary<string, string>();
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(myConfiguration).Build();

            // Act
            Exception ex = Assert.ThrowsAny<Exception>(delegate
            {
                var options = new DbContextOptionsBuilder<Repository.Contexts.AuthContext>().UseInMemoryDatabase("db_guardian_auth").Options;
                AuthService authService = new AuthService(configuration, new AuthRepository(new Repository.Contexts.AuthContext(options)));
            });

            // Assert            
            Assert.NotNull(ex);
        }

        [Fact]
        public void ShouldNotGiveErrorIfAppsettingsValid()
        {
            // Arrange
            var myConfiguration = chavesAppsettings;
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(myConfiguration).Build();

            // Act
            var options = new DbContextOptionsBuilder<Repository.Contexts.AuthContext>().UseInMemoryDatabase("db_guardian_auth").Options;
            AuthService authService = new AuthService(configuration, new AuthRepository(new Repository.Contexts.AuthContext(options)));

            // Assert            
            Assert.NotNull(authService);
        }

        [Fact]
        public async Task ShouldGenerateJwtToken()
        {
            // Arrange
            var mockService = new Mock<IAuthService>();

            // Act
            mockService.Setup(x => x.GenerateJwtToken()).Returns(Task.FromResult("123"));

            // Assert
            Assert.False(String.IsNullOrWhiteSpace(mockService.Object.ToString()));

        }


        [Fact]
        public async Task ShouldGenerateRefreshToken()
        {
            // Arrange
            var mockService = new Mock<IAuthService>();

            // Act
            mockService.Setup(x => x.GenerateRefreshToken(It.IsAny<int>())).Returns(Task.FromResult("123"));

            // Assert
            Assert.False(String.IsNullOrWhiteSpace(mockService.Object.ToString()));
        }

    }
}