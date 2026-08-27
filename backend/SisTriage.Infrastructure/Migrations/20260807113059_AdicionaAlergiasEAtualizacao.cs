using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisTriage.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaAlergiasEAtualizacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Alergias",
                table: "Pacientes",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alergias",
                table: "Pacientes");
        }
    }
}
