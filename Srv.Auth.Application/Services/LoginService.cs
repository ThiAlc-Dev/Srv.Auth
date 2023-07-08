using Srv.Auth.Application.IServices;
using Srv.Auth.CrosCutting.Email;
using Srv.Auth.Domain.Commands;
using Srv.Auth.Domain.Entities;
using Srv.Auth.Domain.Models;
using Srv.Auth.Domain.Responses.CommandResponses;
using Srv.Auth.Repository.IRepositories;

namespace Srv.Auth.Application.Services
{
    public class LoginService : ILoginService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IAuthService _authService;
        private readonly ISmtpEmailClientService _smtpEmailClientService;

        public LoginService(IAuthRepository authRepository, IAuthService authService, ISmtpEmailClientService smtpEmailClientService)
        {
            _authRepository = authRepository;
            _authService = authService;
            _smtpEmailClientService = smtpEmailClientService;
        }

        public async Task<CreateSessionrResponse> LoginAsync(UserModel model)
        {
            ArgumentNullException.ThrowIfNull(nameof(model));

            var cpfCnpj = model.CpfCnpj.Replace("-", "").Replace(".", "").Replace("/", "");
            model.CpfCnpj = cpfCnpj;

            var entity = await _authRepository.GetLoginAsync(model);
            var codeConfirmation = await _authRepository.GetCodeConfirmation(entity.CpfCnpj);

            if (entity != null)
            {
                if (entity.Ativo)
                {
                    return await CreateLoginAsync(model, entity);
                }
                else
                {
                    if (string.IsNullOrEmpty(model.CodigoAcesso) && codeConfirmation.Ativo)
                        return new CreateSessionrResponse
                        {
                            userName = entity.CpfCnpj,
                            authenticated = false,
                            message = "Para o primeiro acesso, entre com o código enviado para seu email.",
                        };

                    if (codeConfirmation.Id > 0 && codeConfirmation.Ativo)
                    {
                        if (codeConfirmation.Validade >= DateTime.Now
                            && codeConfirmation.Codigo == model.CodigoAcesso)
                        {
                            return await CreateLoginAsync(model, entity);
                        }
                        else
                        {
                            // código de acesso inválido ou expirado
                            return new CreateSessionrResponse
                            {
                                authenticated = false,
                                message = "Código inválido/expirado."
                            };
                        }
                    }

                    else
                    {
                        // usuário inativo/bloqueado
                        return new CreateSessionrResponse
                        {
                            authenticated = false,
                            message = "Acesso bloqueado."
                        };
                    }
                }
            }
            else
            {
                // usuário ou senha incorretos
                return new CreateSessionrResponse
                {
                    authenticated = false,
                    message = "Usuário ou senha incorretos."
                };
            }
        }

        public async Task<CreateSessionrResponse> CreateSessionAsync(CreateSessionCommand command)
        {
            ArgumentNullException.ThrowIfNull(typeof(CreateSessionCommand));

            var userModel = new UserModel { CpfCnpj = command.CpfCnpj };
            var user = await _authRepository.GetLoginAsync(userModel);

            if (user != null)
            {
                return await CreateToken(userModel);
            }
            else
            {
                return new CreateSessionrResponse
                {
                    authenticated = false,
                    message = "Erro ao gerar token de acesso!"
                };
            }
        }

        public async Task<ValidateSessionResponse> GetSessionAsync(ValidateSessionCommand command)
        {
            var sessionUser = await _authRepository.GetUserAccessAsync(command.RefreshToken);
            if (sessionUser != null)
            {
                if (sessionUser.DtExpiracaoToken <= DateTime.Now || !sessionUser.Ativo)
                {
                    return new ValidateSessionResponse
                    {
                        Success = false
                    };
                }
                else
                {
                    return new ValidateSessionResponse
                    {
                        Success = true,
                        RefreshToken = sessionUser.Token,
                        DataExpiracao = sessionUser.DtExpiracaoToken.ToString()
                    };
                }
            }
            else
            {
                return new ValidateSessionResponse
                {
                    Success = false
                };
            }

        }

        public async Task<ForgotPasswordResponse> RedefinitionPassword(ForgotPasswordCommand command)
        {
            var responseData = new ForgotPasswordResponse();

            try
            {
                var rdm = new Random();
                var codigo = rdm.Next(1000, 9000);

                var userModel = new UserModel
                {
                    CpfCnpj = command.CpfCnpj,
                    Email = command.Email,
                    Senha = await _authService.GetHashString(command.Senha)
                };

                //var getLogin = await _authRepository.GetLoginAsync(userModel);
                //if (getLogin != null)
                //{
                    await _authRepository.ResetPasswordUser(userModel);

                    // envia email com código de verificação
                    await _smtpEmailClientService.SendEmailAsync(userModel.Email, "Redefinição de senha", $"Código para validação da alteração de senha {codigo}");

                    // grava o codigo gerado associado ao usuário
                    await _authRepository.SaveCodeUser(codigo, userModel.CpfCnpj);

                    responseData.Success = true;
                    responseData.Message = "Senha atualizada com sucesso! Em breve você receberá um email com um código de verificação.";
                //}
                //else
                //{
                //    responseData.Success = false;
                //    responseData.Message = "Usuário não encontrado.";
                //}
            }
            catch (Exception ex)
            {
                responseData.Success = false;
                responseData.Message = ex.Message;
            }

            return responseData;
        }

        private async Task<CreateSessionrResponse> CreateToken(UserModel entity)
        {
            var access = await _authService.GenerateJwtToken(new CreateSessionCommand { CpfCnpj = entity.CpfCnpj });
            var user = await _authRepository.GetLoginAsync(new UserModel { CpfCnpj = entity.CpfCnpj });

            return new CreateSessionrResponse
            {
                acessToken = access.acessToken,
                authenticated = access.authenticated,
                created = access.created,
                expiration = access.expiration,
                message = access.message,
                refreshToken = access.refreshToken,
                userName = $"{access.userName}-{user.Nome}"
            };
        }

        private async Task<CreateSessionrResponse> CreateLoginAsync(UserModel model, UserModel entity)
        {
            if (await _authService.VerifyHash(model.Senha, entity.Senha)
                        && entity.CpfCnpj.Equals(model.CpfCnpj))
            {
                // ativo o usuário ao efetivar o login
                if (!entity.Ativo)
                    await _authRepository.UpdateUserAsync(new UserModel { Ativo = true, CpfCnpj = entity.CpfCnpj });
                
                return await CreateToken(entity);
            }
            else
            {
                // senha incorreta
                return new CreateSessionrResponse
                {
                    userName = entity.CpfCnpj,
                    authenticated = false,
                    message = "Senha incorreta."
                };
            }
        }

        public async Task<DataResponse> GetUserAllAsync()
        {
            var dataResponse = new DataResponse();
            try
            {
                var result = await _authRepository.GeUserAllAsync();
                dataResponse.Success = true;
                dataResponse.Data = result.Select(a => new GetUserModel
                {
                    CpfCnpj = a.CpfCnpj,
                    Email = a.Email,
                    Nome = a.Nome,
                    Status = a.Ativo ? "Ativo" : "Inativo",
                    CriadoEm = a.CriadoEm,
                    AtualizadoEm = a.AtualizadoEm,
                });
            }
            catch (Exception ex)
            {
                dataResponse.Message = ex.Message;
                dataResponse.Success = false;
            }

            return dataResponse;
        }

        public async Task<DataResponse> EditUserAsync(EditUserCommand command)
        {
            var dataResponse = new DataResponse();
            try
            {
                var userModel = new UserModel
                {
                    Ativo = Convert.ToBoolean(command.Status),
                    CpfCnpj = command.CpfCnpj,
                    Email = command.Email,
                    Nome = command.Nome
                };

                var isSeccess = await _authRepository.EditUserAsync(userModel);
                dataResponse.Success = isSeccess;
                dataResponse.Message = "Usuário alterado com sucesso.";
            }
            catch (Exception ex)
            {
                dataResponse.Success = false;
                dataResponse.Message = ex.Message;
            }

            return dataResponse;
        }
    }
}
