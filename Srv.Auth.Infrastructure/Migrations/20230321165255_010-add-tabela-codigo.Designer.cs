﻿// <auto-generated />
using System;
using Srv.Auth.Repository.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Srv.Auth.Repository.Migrations
{
    [DbContext(typeof(AuthContext))]
    [Migration("20230321165255_010-add-tabela-codigo")]
    partial class _010addtabelacodigo
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Srv.Auth.Domain.Entities.User", b =>
                {
                    b.Property<string>("CpfCnpj")
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("Ativo")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("AtualizadoEm")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.HasKey("CpfCnpj");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Usuario", (string)null);

                    b.HasData(
                        new
                        {
                            CpfCnpj = "12276789751",
                            Ativo = false,
                            CriadoEm = new DateTime(2023, 3, 21, 13, 52, 55, 686, DateTimeKind.Local).AddTicks(4472),
                            Email = "thiago.prog@outlook.com",
                            Id = new Guid("e5a3f643-b02a-4ba2-81fb-4f75dd26d4ca"),
                            Nome = "Thiago Alcantara",
                            Senha = "A574DE14EEEB439CF1CC62B7468A71B8F3E58348B662F96D5590A7490B2DDD5A"
                        });
                });

            modelBuilder.Entity("Srv.Auth.Domain.Entities.UserAccess", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Ativo")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("DtCriacaoToken")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DtExpiracaoToken")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UsuarioCpfCnpj")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioCpfCnpj");

                    b.ToTable("UsuarioSessao", (string)null);
                });

            modelBuilder.Entity("Srv.Auth.Domain.Entities.UserCodeConfirmation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Codigo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UsuarioCpfCnpj")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("Validade")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioCpfCnpj");

                    b.ToTable("CodigoConfimacaoUsuario", (string)null);
                });

            modelBuilder.Entity("Srv.Auth.Domain.Entities.UserAccess", b =>
                {
                    b.HasOne("Srv.Auth.Domain.Entities.User", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioCpfCnpj")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Srv.Auth.Domain.Entities.UserCodeConfirmation", b =>
                {
                    b.HasOne("Srv.Auth.Domain.Entities.User", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioCpfCnpj")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });
#pragma warning restore 612, 618
        }
    }
}
