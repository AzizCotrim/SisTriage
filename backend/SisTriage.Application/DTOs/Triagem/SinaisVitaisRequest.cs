using System.ComponentModel.DataAnnotations;

namespace SisTriage.Application.DTOs.Triagem
{
    public class SinaisVitaisRequest
    {
        [Range(40, 300, ErrorMessage = "A pressão arterial sistólica deve estar entre 40 e 300 mmHg.")]
        public required int PressaoArterialSistolica { get; set; }

        [Range(20, 200, ErrorMessage = "A pressão arterial diastólica deve estar entre 20 e 200 mmHg.")]
        public required int PressaoArterialDiastolica { get; set; }

        [Range(20, 250, ErrorMessage = "A frequência cardíaca deve estar entre 20 e 250 bpm.")]
        public required int FrequenciaCardiaca { get; set; }

        [Range(4, 80, ErrorMessage = "A frequência respiratória deve estar entre 4 e 80 irpm.")]
        public required int FrequenciaRespiratoria { get; set; }

        [Range(25.0, 45.0, ErrorMessage = "A temperatura deve estar entre 25 e 45 °C.")]
        public required decimal Temperatura { get; set; }

        [Range(50, 100, ErrorMessage = "A saturação de oxigênio deve estar entre 50 e 100%.")]
        public required int SpO2 { get; set; }

        [Range(0, 10, ErrorMessage = "A escala de dor deve ser de 0 a 10.")]
        public required int EscalaDor { get; set; }

        [Range(10, 1000, ErrorMessage = "A glicemia deve estar entre 10 e 1000 mg/dL.")]
        public int? Glicemia { get; set; }

        [Range(0.5, 400.0, ErrorMessage = "O peso deve estar entre 0,5 e 400 kg.")]
        public decimal? Peso { get; set; }
    }
}
