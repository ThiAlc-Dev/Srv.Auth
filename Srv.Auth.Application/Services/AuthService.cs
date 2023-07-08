using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Srv.Auth.Domain.Commands;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Srv.Auth.Application.IServices;
using Srv.Auth.Repository.IRepositories;
using Microsoft.Extensions.Options;
using Srv.Auth.Repository.Options;
using Srv.Auth.Domain.Responses.CommandResponses;
using Srv.Auth.Domain.Models;
using Srv.Auth.CrosCutting.Email;

namespace Srv.Auth.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IOptions<JwtOptions> _options;
        private readonly ISmtpEmailClientService _smtpEmailClientService;

        public AuthService(IOptions<JwtOptions> options, IAuthRepository authRepository, ISmtpEmailClientService smtpEmailClientService)
        {
            _authRepository = authRepository;
            _options = options;
            _smtpEmailClientService = smtpEmailClientService;
        }

        public async Task<ForgotPasswordResponse> CreateUserAsync(CreateUserCommand command)
        {
            var cpfCnpj = command.CpfCnpj.Replace("-", "").Replace(".", "").Replace("/", "");
            var result = await _authRepository.CreateUserAsync(new UserModel
            {
                CpfCnpj = cpfCnpj,
                Email = command.Email,
                Nome = command.Nome,
                Senha = await GetHashString(command.Senha)
            });

            // envia email com código para primeiro acesso
            var rdm = new Random();
            var codigo = rdm.Next(1000, 9000);
            await _smtpEmailClientService.SendEmailAsync(result.Email, "Primeiro Acesso", $"Para seu primeiro acesso é necessário digitar o código: {codigo}");

            // sava o código no banco
            await _authRepository.SaveCodeUser(codigo, cpfCnpj);

            return new ForgotPasswordResponse
            {
                Message = "Cadastrado com sucesso",
                Success = true
            };
        }

        #region Token
        public async Task<CreateSessionrResponse> GenerateJwtToken(CreateSessionCommand command)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("CpfCnpj", command.CpfCnpj)
            };

            var createDate = DateTime.Now;
            var expiredDate = DateTime.Now.AddMinutes(int.Parse(_options.Value.AccessTokenExpirationInMinutes));
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Key + ""));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var accessTokenDescriptor = new JwtSecurityToken(
                issuer: _options.Value.Issuer,
                audience: _options.Value.Audience,
                claims: claims,
                expires: expiredDate,
                notBefore: createDate,
                signingCredentials: credentials);

            string jwtAccessToken = new JwtSecurityTokenHandler().WriteToken(accessTokenDescriptor);
            var refreshToken = GenerateRefreshToken(command.CpfCnpj).RefreshToken;

            await UpdateOldTokens(command.CpfCnpj);
            await SaveNewTokenData(refreshToken, command.CpfCnpj);

            //succsses
            return new CreateSessionrResponse
            {
                authenticated = true,
                created = createDate.ToString("yyyy-MM-dd HH:mm:ss"),
                expiration = expiredDate.ToString("yyyy-MM-dd HH:mm:ss"),
                acessToken = jwtAccessToken,
                refreshToken = refreshToken,
                userName = command.CpfCnpj,
                message = "Usuário autenticado."
            };
        }

        private RefreshTokenModel GenerateRefreshToken(string userEmail)
        {
            var refreshToken = new RefreshTokenModel
            {
                Username = userEmail,
                ExpirationDate = _options.Value.AccessTokenExpirationInMinutes
            };

            string token;
            var randomNumber = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                token = Convert.ToBase64String(randomNumber);
            }

            refreshToken.RefreshToken = token
                .Replace("+", string.Empty)
                .Replace("=", string.Empty)
                .Replace("/", string.Empty);

            return refreshToken;
        }

        #endregion

        #region Auxiliar methods

        private static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
            {
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            }
        }

        public Task<string> GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
            {
                sb.Append(b.ToString("X2"));
            }
            return Task.FromResult(sb.ToString());
        }

        public async Task<bool> VerifyHash(string password, string bdHash)
        {
            var newHash = await GetHashString(password);
            return bdHash == newHash;
        }

        private async Task UpdateOldTokens(string cpfCnpj)
        {
            await _authRepository.UpdateSessionUserAsync(cpfCnpj);
        }

        private async Task SaveNewTokenData(string jwtRefreshToken, string cpfCnpj)
        {
            UserAccessModel token = new UserAccessModel()
            {
                CpfCnpj = cpfCnpj,
                Token = jwtRefreshToken,
                DtCriacaoToken = DateTime.Now,
                DtExpiracaoToken = DateTime.Now.AddMinutes(int.Parse(_options.Value.AccessTokenExpirationInMinutes)),
                Ativo = true
            };

            await _authRepository.SaveSessionToken(token);

            #endregion
        }
    }
}
