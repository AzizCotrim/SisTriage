namespace SisTriage.Application.DTOs.Indicador
{
    public class IndicadoresResponse
    {
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }

        public int TotalRegistros { get; set; }
        public int Atendidos { get; set; }
        public int NaoComparecimentos { get; set; }
        public int AindaNaFila { get; set; }

        /// <summary>
        /// Média calculada apenas sobre atendimentos concluídos.
        /// </summary>
        public double TempoMedioEsperaMinutos { get; set; }

        public int DentroDoTempo { get; set; }
        public int ForaDoTempo { get; set; }

        /// <summary>
        /// Percentual de atendimentos que respeitaram o tempo-alvo do Protocolo de Manchester.
        /// </summary>
        public double PercentualDentroDoTempo { get; set; }

        public required IEnumerable<IndicadorPorClassificacaoResponse> PorClassificacao { get; set; }
    }
}
