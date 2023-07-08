using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Srv.Auth.Repository.Migrations
{
    /// <inheritdoc />
    public partial class _002addTableusuariosessao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsuarioSessao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Token = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DtCriacaoToken = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DtExpiracaoToken = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UsuarioCpfCnpj = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioSessao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioSessao_Usuario_UsuarioCpfCnpj",
                        column: x => x.UsuarioCpfCnpj,
                        principalTable: "Usuario",
                        principalColumn: "CpfCnpj",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "CpfCnpj",
                keyValue: "12276789751",
                columns: new[] { "CriadoEm", "Id" },
                values: new object[] { new DateTime(2023, 2, 27, 13, 32, 56, 542, DateTimeKind.Utc).AddTicks(1518), new Guid("ccae6ad0-04bf-4c46-b3d9-c3a4ad3a4499") });

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioSessao_UsuarioCpfCnpj",
                table: "UsuarioSessao",
                column: "UsuarioCpfCnpj");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuarioSessao");

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "CpfCnpj",
                keyValue: "12276789751",
                columns: new[] { "CriadoEm", "Id" },
                values: new object[] { new DateTime(2023, 2, 27, 13, 31, 50, 575, DateTimeKind.Utc).AddTicks(477), new Guid("990655ae-243a-447d-95b7-c5e8abce3fdb") });
        }
    }
}
