namespace SisTriage.Application.DTOs.Triagem
{
    public class SinaisVitaisResponse
    {
        public int PressaoArterialSistolica { get; set; }
        public int PressaoArterialDiastolica { get; set; }
        public int FrequenciaCardiaca { get; set; }
        public int FrequenciaRespiratoria { get; set; }
        public decimal Temperatura { get; set; }
        public int SpO2 { get; set; }
        public int EscalaDor { get; set; }
        public int? Glicemia { get; set; }
        public decimal? Peso { get; set; }
    }
}
