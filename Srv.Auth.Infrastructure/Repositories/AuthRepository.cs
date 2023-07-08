using Microsoft.EntityFrameworkCore;
using Srv.Auth.Repository.Contexts;
using Srv.Auth.Repository.IRepositories;
using Srv.Auth.Domain.Entities;
using Srv.Auth.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Validations;

namespace Srv.Auth.Repository.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AuthContext _context;
        private readonly ILogger<AuthContext> _logger;

        public AuthRepository(AuthContext context, ILogger<AuthContext> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<UserModel> GetLoginAsync(UserModel userModel)
        {
            try
            {
                var entity = _context.Users.Where(a =>
                                (a.CpfCnpj == userModel.CpfCnpj))
                            .AsNoTracking().FirstOrDefault();

                return entity != null ? new UserModel
                {
                    Id = entity.Id,
                    Email = entity.Email,
                    CpfCnpj = entity.CpfCnpj,
                    Nome = entity.Nome,
                    Senha = entity.Senha,
                    Ativo = entity.Ativo
                } : new UserModel();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetLoginAsync: Erro ao obter o login.");
                return new UserModel();
            }
        }

        public async Task<UserModel> CreateUserAsync(UserModel userModel)
        {
            try
            {
                var entity = new User
                {
                    CpfCnpj = userModel.CpfCnpj,
                    Nome = userModel.Nome,
                    Email = userModel.Email,
                    Senha = userModel.Senha,
                    Ativo = false
                };

                _context.Users.Add(entity);
                await _context.SaveChangesAsync();

                return userModel;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateUserAsync: Erro ao criar usuário.");
                return new UserModel();
            }
        }

        public async Task<bool> SaveSessionToken(UserAccessModel userAccessModel)
        {
            try
            {
                var entity = new UserAccess
                {
                    Ativo = true,
                    DtCriacaoToken = userAccessModel.DtCriacaoToken,
                    DtExpiracaoToken = userAccessModel.DtExpiracaoToken,
                    Token = userAccessModel.Token,
                    UsuarioCpfCnpj = userAccessModel.CpfCnpj,
                    Id = Guid.NewGuid(),
                };

                _context.UserAccesses.Add(entity);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SaveSessionToken: Erro ao salvar token da sessão.");
                return false;
            }
        }

        public async Task<UserAccessModel> GetUserAccessAsync(string refreshToken)
        {
            try
            {
                var entity = _context.UserAccesses.Where(u => u.Token.Equals(refreshToken)).FirstOrDefault();

                return entity != null ? new UserAccessModel
                {
                    Token = entity.Token,
                    Ativo = entity.Ativo,
                    CpfCnpj = entity.UsuarioCpfCnpj,
                    DtCriacaoToken = entity.DtCriacaoToken,
                    DtExpiracaoToken = entity.DtExpiracaoToken
                } : new UserAccessModel();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetSessionAsync: Erro ao obter a sessão do usuário.");
                return new UserAccessModel();
            }
        }

        public async Task UpdateSessionUserAsync(string cpfCnpj)
        {
            using var transition = _context.Database.BeginTransaction();

            await _context.Database.ExecuteSqlInterpolatedAsync($"UPDATE UsuarioSessao SET Ativo = 0 WHERE UsuarioCpfCnpj = {cpfCnpj}");

            //var entity = _context.UserAccesses
            //    .Where(u => u.UsuarioCpfCnpj == cpfCnpj)
            //    .ExecuteUpdateAsync(u => u.SetProperty(a => a.Ativo, a => false));

            //await _context.SaveChangesAsync();

            await transition.CommitAsync();
            await transition.DisposeAsync();
        }

        public async Task ResetPasswordUser(UserModel userModel)
        {
            try
            {
                var entity = _context.Users.Where(a => a.CpfCnpj == userModel.CpfCnpj).FirstOrDefault();

                if (entity != null)
                {
                    entity.CpfCnpj = userModel.CpfCnpj;
                    entity.Email = userModel.Email;
                    entity.Senha = userModel.Senha;
                    // só ativa ao confirmar o código recebido no email no ato da redefinição de senha
                    entity.Ativo = false;
                    entity.AtualizadoEm = DateTime.Now;

                    _context.ChangeTracker.DetectChanges();
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Usuário não encontrado.");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateUserAsync: Erro ao criar usuário.");
            }
        }

        public async Task SaveCodeUser(int codigo, string cpfCnpj)
        {
            _context.UserCodeConfirmation.Add(new UserCodeConfirmation
            {
                Codigo = codigo.ToString(),
                UsuarioCpfCnpj = cpfCnpj,
                Validade = DateTime.Now.AddMinutes(10)
            });

            await _context.SaveChangesAsync();
        }

        public async Task<UserCodeConfirmationModel> GetCodeConfirmation(string cpfCnpj)
        {
            var result = await _context.UserCodeConfirmation.Where(a => a.UsuarioCpfCnpj == cpfCnpj).OrderByDescending(o => o.Validade).AsNoTracking().FirstOrDefaultAsync();

            return result != null ? new UserCodeConfirmationModel
            {
                Codigo = result.Codigo,
                Id = result.Id,
                UsuarioCpfCnpj = result.UsuarioCpfCnpj,
                Validade = result.Validade,
                Ativo = result.Validade >= DateTime.Now
        } : new UserCodeConfirmationModel();
        }

        public async Task UpdateUserAsync(UserModel userModel)
        {
            var entity = _context.Users.Where(a => a.CpfCnpj == userModel.CpfCnpj).FirstOrDefault();
            if (entity != null)
            {
                entity.Nome = string.IsNullOrEmpty(userModel.Nome) ? entity.Nome : userModel.Nome;
                entity.Ativo = userModel.Ativo;

                _context.ChangeTracker.DetectChanges();
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> GeUserAllAsync()
        {
            var result = await _context.Users.ToListAsync();
            return result;
        }

        public async Task<bool> EditUserAsync(UserModel userModel)
        {
            try
            {
                var entiy = await _context.Users.Where(a => a.CpfCnpj == userModel.CpfCnpj).FirstOrDefaultAsync();
                if (entiy == null)
                {
                    return false;
                }
                else
                {
                    entiy.Nome = userModel.Nome;
                    entiy.Email = userModel.Email;
                    entiy.Ativo = userModel.Ativo;
                    entiy.AtualizadoEm = DateTime.Now;

                    _context.ChangeTracker.DetectChanges();
                    await _context.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
    }
}