using MediatR;
using Srv.Auth.Application.Handlers.CommandHandlers;
using Srv.Auth.Application.Services;
using Srv.Auth.Domain.Commands;
using System.Reflection;
using Srv.Auth.Application.IServices;
using Srv.Auth.Application.Handlers.QueryHandlers;
using Srv.Auth.Domain.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Srv.Auth.Repository.Configurations;
using Srv.Auth.Repository.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AssSmtpClientEmail(builder.Configuration);
builder.Services.AddMySqlConfigurations(builder.Configuration);

InitConfigureOptions();
InitServicesDependencyInjection();
InitMediatr();
InitSwagger();
IniteAuthentication();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();
//app.UseMiddleware<JwtMiddleware>();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();


app.Run();


void InitMediatr()
{
    builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
    builder.Services.AddMediatR(typeof(CreateUserCommand).Assembly);
    builder.Services.AddMediatR(typeof(CreateUserCommandHandler).Assembly);

    builder.Services.AddMediatR(typeof(GetLoginQuery).Assembly);
    builder.Services.AddMediatR(typeof(GetLoginQueryHandler).Assembly);
}

void InitSwagger()
{
    builder.Services.AddSwaggerConfigurations();
}

void InitServicesDependencyInjection()
{
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ILoginService, LoginService>();
}

void InitConfigureOptions()
{
    builder.Services.Configure<AppSetingsOptions>(builder.Configuration.GetSection("ConnectionStrings"));
    builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
}

void IniteAuthentication()
{
    builder.Services.AddAuthentication
                 (JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,

                         ValidIssuer = builder.Configuration["Jwt:Issuer"],
                         ValidAudience = builder.Configuration["Jwt:Audience"],
                         IssuerSigningKey = new SymmetricSecurityKey
                       (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                     };
                 });
}

