using Srv.Auth.Application.IServices;
using Srv.Auth.Domain.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Srv.Auth.Api.Attributes
{
    public class ValidateSessionTokenAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Microsoft.Extensions.Primitives.StringValues headerValuesRefreshToken;
            var refreshToken = context.HttpContext.Request.Headers.TryGetValue("RefreshToken", out headerValuesRefreshToken);

            if (!refreshToken)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Não Autorizado"
                };
                return;
            }
            else
            {
                try
                {
                    var sessioniModel = new ValidateSessionCommand();
                    var _loginService = context.HttpContext.RequestServices.GetRequiredService<ILoginService>();

                    sessioniModel.RefreshToken = headerValuesRefreshToken.First()!;
                    var validate = await _loginService.GetSessionAsync(sessioniModel);

                    if (validate.Success && !string.IsNullOrEmpty(validate.RefreshToken))
                        context.HttpContext.Response.Headers.Add("RefreshToken", validate.RefreshToken);
                    else
                    {
                        context.Result = new ContentResult()
                        {
                            StatusCode = 401,
                            Content = "Não Autorizado"
                        };
                        return;
                    }

                }
                catch (Exception ex)
                {

                    throw;
                }
            }

            await next();
        }
    }
}
