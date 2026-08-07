using SisTriage.Domain.Enums;

namespace SisTriage.Application.DTOs.Paciente
{
    public class PacienteResponse
    {
        public Guid Id { get; set; }
        public required string Nome { get; set; }
        public required string CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public Sexo Sexo { get; set; }
        public required string Telefone { get; set; }
        public string? Alergias { get; set; }
    }
}
