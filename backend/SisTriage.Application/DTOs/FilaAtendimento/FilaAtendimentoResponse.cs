using SisTriage.Domain.Enums;

namespace SisTriage.Application.DTOs.FilaAtendimento
{
    public class FilaAtendimentoResponse
    {
        public Guid Id { get; set; }
        public required string PacienteNome { get; set; }
        public ClassificacaoRisco Classificacao { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime? DataSaida { get; set; }
        public StatusFila Status { get; set; }

        /// <summary>
        /// Tempo já aguardado. Para quem ainda está na fila, conta até agora;
        /// para quem já saiu, conta até a saída.
        /// </summary>
        public int TempoEsperaMinutos { get; set; }

        /// <summary>
        /// Tempo-alvo do Protocolo de Manchester para a classificação de risco.
        /// </summary>
        public int TempoMaximoEsperaMinutos { get; set; }

        public bool TempoExcedido => TempoEsperaMinutos > TempoMaximoEsperaMinutos;
    }
}
