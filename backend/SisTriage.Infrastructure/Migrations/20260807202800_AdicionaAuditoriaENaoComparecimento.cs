using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisTriage.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaAuditoriaENaoComparecimento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogsAuditoria",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: true),
                    UsuarioEmail = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Acao = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Entidade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RegistroId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DataHora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogsAuditoria", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogsAuditoria_DataHora",
                table: "LogsAuditoria",
                column: "DataHora");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogsAuditoria");
        }
    }
}
