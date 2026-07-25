using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisTriage.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    CPF = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Sexo = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    Telefone = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    SenhaHash = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Perfil = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Triagens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PacienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataHora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Classificacao = table.Column<string>(type: "text", nullable: false),
                    QueixaPrincipal = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Triagens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Triagens_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Triagens_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FilaAtendimentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TriagemId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataEntrada = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataSaida = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilaAtendimentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FilaAtendimentos_Triagens_TriagemId",
                        column: x => x.TriagemId,
                        principalTable: "Triagens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilaAtendimentos_TriagemId",
                table: "FilaAtendimentos",
                column: "TriagemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_CPF",
                table: "Pacientes",
                column: "CPF",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Triagens_PacienteId",
                table: "Triagens",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Triagens_UsuarioId",
                table: "Triagens",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilaAtendimentos");

            migrationBuilder.DropTable(
                name: "Triagens");

            migrationBuilder.DropTable(
                name: "Pacientes");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
