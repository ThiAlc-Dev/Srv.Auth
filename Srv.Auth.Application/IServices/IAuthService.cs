using Srv.Auth.Domain.Commands;
using Srv.Auth.Domain.Responses.CommandResponses;

namespace Srv.Auth.Application.IServices
{
    public interface IAuthService
    {
        Task<bool> VerifyHash(string password, string bdHash);
        Task<ForgotPasswordResponse> CreateUserAsync(CreateUserCommand command);
        Task<CreateSessionrResponse> GenerateJwtToken(CreateSessionCommand command);
        Task<string> GetHashString(string inputString);

    }
}
