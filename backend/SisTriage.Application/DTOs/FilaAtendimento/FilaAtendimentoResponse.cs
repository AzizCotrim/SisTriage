using SisTriage.Domain.Enums;

namespace SisTriage.Application.DTOs.FilaAtendimento
{
    public class FilaAtendimentoResponse
    {
        public Guid Id { get; set; }
        public required string PacienteNome { get; set; }
        public ClassificacaoRisco Classificacao { get; set; }
        public DateTime DataEntrada { get; set; }
        public StatusFila Status { get; set; }
    }
}
