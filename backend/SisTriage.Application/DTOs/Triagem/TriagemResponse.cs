using SisTriage.Domain.Enums;

namespace SisTriage.Application.DTOs.Triagem
{
    public class TriagemResponse
    {
        public Guid Id { get; set; }
        public required string PacienteNome { get; set; }
        public required string UsuarioNome { get; set; }
        public DateTime DataHora { get; set; }
        public ClassificacaoRisco Classificacao { get; set; }
        public required string QueixaPrincipal { get; set; }
        public StatusTriagem Status { get; set; }
        public required SinaisVitaisResponse SinaisVitais { get; set; }
    }
}
