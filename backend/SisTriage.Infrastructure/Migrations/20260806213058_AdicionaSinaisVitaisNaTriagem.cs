using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SisTriage.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaSinaisVitaisNaTriagem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SinaisVitais_EscalaDor",
                table: "Triagens",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SinaisVitais_FrequenciaCardiaca",
                table: "Triagens",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SinaisVitais_FrequenciaRespiratoria",
                table: "Triagens",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SinaisVitais_Glicemia",
                table: "Triagens",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SinaisVitais_Peso",
                table: "Triagens",
                type: "numeric(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SinaisVitais_PressaoArterialDiastolica",
                table: "Triagens",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SinaisVitais_PressaoArterialSistolica",
                table: "Triagens",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SinaisVitais_SpO2",
                table: "Triagens",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "SinaisVitais_Temperatura",
                table: "Triagens",
                type: "numeric(4,1)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SinaisVitais_EscalaDor",
                table: "Triagens");

            migrationBuilder.DropColumn(
                name: "SinaisVitais_FrequenciaCardiaca",
                table: "Triagens");

            migrationBuilder.DropColumn(
                name: "SinaisVitais_FrequenciaRespiratoria",
                table: "Triagens");

            migrationBuilder.DropColumn(
                name: "SinaisVitais_Glicemia",
                table: "Triagens");

            migrationBuilder.DropColumn(
                name: "SinaisVitais_Peso",
                table: "Triagens");

            migrationBuilder.DropColumn(
                name: "SinaisVitais_PressaoArterialDiastolica",
                table: "Triagens");

            migrationBuilder.DropColumn(
                name: "SinaisVitais_PressaoArterialSistolica",
                table: "Triagens");

            migrationBuilder.DropColumn(
                name: "SinaisVitais_SpO2",
                table: "Triagens");

            migrationBuilder.DropColumn(
                name: "SinaisVitais_Temperatura",
                table: "Triagens");
        }
    }
}
