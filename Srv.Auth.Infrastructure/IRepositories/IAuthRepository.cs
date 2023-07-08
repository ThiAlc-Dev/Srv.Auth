using Srv.Auth.Domain.Entities;
using Srv.Auth.Domain.Models;

namespace Srv.Auth.Repository.IRepositories
{
    public interface IAuthRepository
    {
        Task<UserModel> GetLoginAsync(UserModel entity);
        Task<UserModel> CreateUserAsync(UserModel userModel);
        Task<bool> SaveSessionToken(UserAccessModel userAccessModel);
        Task<UserAccessModel> GetUserAccessAsync(string refreshToken);
        Task UpdateSessionUserAsync(string cpfCnpj);
        Task ResetPasswordUser(UserModel userModel);
        Task SaveCodeUser(int codigo, string cpfCnpj);
        Task<UserCodeConfirmationModel> GetCodeConfirmation(string cpfCnpj);
        Task UpdateUserAsync(UserModel userModel);
        Task<IEnumerable<User>> GeUserAllAsync();
        Task<bool> EditUserAsync(UserModel userModel);
    }
}
