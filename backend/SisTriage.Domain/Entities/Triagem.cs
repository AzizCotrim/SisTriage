using SisTriage.Domain.Enums;

namespace SisTriage.Domain.Entities
{
    public class Triagem
    {
        public Guid Id { get; private set; }
        public required Guid PacienteId { get; set; }
        public Paciente Paciente { get; set; } = null!;
        public required Guid UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
        public required DateTime DataHora { get; set; }
        public ClassificacaoRisco Classificacao { get; set; }
        public required string QueixaPrincipal { get; set; }
        public StatusTriagem Status { get; set; }

        public Triagem()
        {
            Id = Guid.NewGuid();
        }

    }
}
