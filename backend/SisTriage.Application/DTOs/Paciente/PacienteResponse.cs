namespace SisTriage.Application.DTOs.Paciente
{
    public class PacienteResponse
    {
        public Guid Id { get; set; }
        public required string Nome { get; set; }
        public required string CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public required string Sexo { get; set; }
        public required string Telefone { get; set; }
    }
}
