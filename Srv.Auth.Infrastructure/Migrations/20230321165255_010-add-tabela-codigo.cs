using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Srv.Auth.Repository.Migrations
{
    public partial class _010addtabelacodigo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CodigoConfimacaoUsuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UsuarioCpfCnpj = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Codigo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Validade = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodigoConfimacaoUsuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodigoConfimacaoUsuario_Usuario_UsuarioCpfCnpj",
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
                values: new object[] { new DateTime(2023, 3, 21, 13, 52, 55, 686, DateTimeKind.Local).AddTicks(4472), new Guid("e5a3f643-b02a-4ba2-81fb-4f75dd26d4ca") });

            migrationBuilder.CreateIndex(
                name: "IX_CodigoConfimacaoUsuario_UsuarioCpfCnpj",
                table: "CodigoConfimacaoUsuario",
                column: "UsuarioCpfCnpj");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodigoConfimacaoUsuario");

            migrationBuilder.UpdateData(
                table: "Usuario",
                keyColumn: "CpfCnpj",
                keyValue: "12276789751",
                columns: new[] { "CriadoEm", "Id" },
                values: new object[] { new DateTime(2023, 3, 21, 13, 40, 13, 668, DateTimeKind.Local).AddTicks(7528), new Guid("35e94705-7041-477c-9116-9e633d92f1fc") });
        }
    }
}
