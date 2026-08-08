using SisTriage.Domain.Enums;

namespace SisTriage.Application.DTOs.Indicador
{
    public class IndicadorPorClassificacaoResponse
    {
        public ClassificacaoRisco Classificacao { get; set; }
        public int TempoMaximoEsperaMinutos { get; set; }
        public int Quantidade { get; set; }
        public double TempoMedioEsperaMinutos { get; set; }
        public int DentroDoTempo { get; set; }
        public int ForaDoTempo { get; set; }
    }
}
