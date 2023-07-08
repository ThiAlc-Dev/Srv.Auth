using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Srv.Auth.Repository.Contexts
{
    public class AuthContextFactory : IDesignTimeDbContextFactory<AuthContext>
    {
        public AuthContext CreateDbContext(string[] args)
        {
            //var conn = "server=mysql.grupopca.kinghost.net;port=3306;database=grupopca;uid=grupopca;password=gpca134679";
            var conn = "";
            var version = "10.6";

            var optionsBuilder = new DbContextOptionsBuilder<AuthContext>();
            optionsBuilder.UseMySql(conn, new MySqlServerVersion(new Version(version)))
                    .EnableSensitiveDataLogging(true)
                    .EnableDetailedErrors();

            return new AuthContext(optionsBuilder.Options);
        }
    }
}
