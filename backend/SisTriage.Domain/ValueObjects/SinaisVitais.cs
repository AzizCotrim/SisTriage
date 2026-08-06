namespace SisTriage.Domain.ValueObjects
{
    public class SinaisVitais
    {
        public required int PressaoArterialSistolica { get; set; }
        public required int PressaoArterialDiastolica { get; set; }
        public required int FrequenciaCardiaca { get; set; }
        public required int FrequenciaRespiratoria { get; set; }
        public required decimal Temperatura { get; set; }
        public required int SpO2 { get; set; }
        public required int EscalaDor { get; set; }
        public int? Glicemia { get; set; }
        public decimal? Peso { get; set; }
    }
}
