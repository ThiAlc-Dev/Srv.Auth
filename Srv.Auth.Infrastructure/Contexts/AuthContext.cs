using Microsoft.EntityFrameworkCore;
using Srv.Auth.Domain.Entities;

namespace Srv.Auth.Repository.Contexts
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = default!;
        public DbSet<UserAccess> UserAccesses { get; set; } = default!;
        public DbSet<UserCodeConfirmation> UserCodeConfirmation { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Usuario");
            modelBuilder.Entity<User>().HasKey(u => u.CpfCnpj);
            modelBuilder.Entity<User>().HasIndex(u => u.Id).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<User>().Property(u => u.Nome).IsRequired().HasMaxLength(150);
            modelBuilder.Entity<User>().Property(u => u.Email).IsRequired().HasMaxLength(150);
            modelBuilder.Entity<User>().Property(u => u.Senha).IsRequired().HasMaxLength(250);
            modelBuilder.Entity<User>().HasData(new User
            {
                CpfCnpj = "12276789751",
                Nome = "Thiago Alcantara",
                Email = "thiago.prog@outlook.com",
                Senha = "A574DE14EEEB439CF1CC62B7468A71B8F3E58348B662F96D5590A7490B2DDD5A",
            });
            //modelBuilder.Entity<User>().HasOne(u => u.CodigoConfimacaoUsuario).WithMany();

            modelBuilder.Entity<UserAccess>().ToTable("UsuarioSessao");
            modelBuilder.Entity<UserAccess>().HasKey(u => u.Id);
            modelBuilder.Entity<UserAccess>().HasOne(u => u.Usuario);

            modelBuilder.Entity<UserCodeConfirmation>().ToTable("CodigoConfimacaoUsuario");
            modelBuilder.Entity<UserCodeConfirmation>().HasKey(a => a.Id);
        }
    }
}