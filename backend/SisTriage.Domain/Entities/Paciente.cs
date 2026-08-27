using SisTriage.Domain.Enums;

namespace SisTriage.Domain.Entities
{
    public class Paciente
    {
        public Guid Id { get; private set; }
        public required string Nome { get; set; }
        public required string CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public required Sexo Sexo { get; set; }
        public required string Telefone { get; set; }
        public string? Alergias { get; set; }

        public ICollection<Triagem> Triagens { get; set; } = new List<Triagem>();

        public Paciente()
        {
            Id = Guid.NewGuid();
        }
    }
}
