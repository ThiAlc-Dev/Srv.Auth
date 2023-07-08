using Srv.Auth.Domain.Commands;
using Srv.Auth.Domain.Models;
using Srv.Auth.Domain.Responses.CommandResponses;

namespace Srv.Auth.Application.IServices
{
    public interface ILoginService
    {
        Task<CreateSessionrResponse> LoginAsync(UserModel entity);
        Task<CreateSessionrResponse> CreateSessionAsync(CreateSessionCommand command);
        Task<ValidateSessionResponse> GetSessionAsync(ValidateSessionCommand command);
        Task<ForgotPasswordResponse> RedefinitionPassword(ForgotPasswordCommand command);
        Task<DataResponse> GetUserAllAsync();
        Task<DataResponse> EditUserAsync(EditUserCommand command);
    }
}
