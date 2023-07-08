using Microsoft.IdentityModel.Tokens;
using Srv.Auth.Application.IServices;
using Srv.Auth.Application.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Srv.Auth.API.Filters
{ 
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly string? _jwtSecret;
        private IAuthService? _authService;
        private readonly IServiceProvider _serviceProvider;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _next = next;
            _configuration = configuration;
            _jwtSecret = _configuration.GetSection("Jwt").GetChildren().SingleOrDefault(c => c.Key == "Key")?.Value;
            _serviceProvider = serviceProvider;  
        }

        public async Task Invoke(HttpContext context, IAuthService authService)
        {
            _authService = authService;

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                await attachUserToContext(context, token);
            }

            await _next(context);
        }
   
        private async Task attachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSecret + String.Empty);


                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                }, out SecurityToken validatedToken);

                JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;

                if (_authService == null) throw new Exception("Serviço AuthService não foi configurado corretamente.");

                var userClaims = jwtToken.Claims;
                var userIdentity = new System.Security.Claims.ClaimsIdentity(userClaims, "Basic");
                var userPrincipal = new System.Security.Claims.ClaimsPrincipal(new[] { userIdentity });
                context.User = userPrincipal;
            }
            catch
            {
            }
        }
    }
}