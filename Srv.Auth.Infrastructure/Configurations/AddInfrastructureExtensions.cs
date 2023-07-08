using Srv.Auth.CrosCutting.Email;
using Srv.Auth.Repository.Contexts;
using Srv.Auth.Repository.IRepositories;
using Srv.Auth.Repository.Options;
using Srv.Auth.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Srv.Auth.Repository.Configurations
{
    public static class AddInfrastructureExtensions
    {
        public static IServiceCollection AddMySqlConfigurations(this IServiceCollection services, IConfiguration configurations)
        {
            ArgumentNullException.ThrowIfNull(nameof(services));
            ArgumentNullException.ThrowIfNull(nameof(configurations));

            services.AddDbContextFactory<AuthContext>(options =>
            {
                options.UseMySql(
                    configurations.GetConnectionString("DbConnectionString"),
                    new MySqlServerVersion(
                        new Version(configurations.GetConnectionString("MySqlVersion")!)
                        )
                    );
            });

            services.AddDbContext<AuthContext>(options =>
            {
                options.UseMySql(
                    configurations.GetConnectionString("DbConnectionString"),
                    new MySqlServerVersion(
                        new Version(configurations.GetConnectionString("MySqlVersion")!)
                        )
                    );
            });

            services.AddTransient<IAuthRepository, AuthRepository>();

            return services;
        }

        public static IServiceCollection AddSwaggerConfigurations(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));

            //configure swagger version
            services.AddApiVersioning(config =>
            {
                config.ReportApiVersions = true;
                config.UseApiBehavior = true;
            });

            services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Token de autenticação.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                 {
                    {
                        new OpenApiSecurityScheme
                            {
                                 Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                                 Name = "Authorization",
                                 In = ParameterLocation.Header,
                                 Type = SecuritySchemeType.Http,
                                 Scheme = "Bearer"
                            },
                            new List<string>()
                    }
                });
            });

            //configure options
            services.ConfigureOptions<SwaggerOptions>();

            return services;

        }

        public static IServiceCollection AssSmtpClientEmail(this IServiceCollection services, IConfiguration configurations)
        {
            services.Configure<EmailSettings>(configurations.GetSection("EmailSettings"));
            services.AddTransient<ISmtpEmailClientService, SmtpEmailClientService>();

            return services;
        }
    }
}
