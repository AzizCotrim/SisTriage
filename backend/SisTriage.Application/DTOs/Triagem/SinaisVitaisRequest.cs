using System.ComponentModel.DataAnnotations;

namespace SisTriage.Application.DTOs.Triagem
{
    public class SinaisVitaisRequest
    {
        [Required(ErrorMessage = "A pressão arterial sistólica é obrigatória.")]
        public required int PressaoArterialSistolica { get; set; }

        [Required(ErrorMessage = "A pressão arterial diastólica é obrigatória.")]
        public required int PressaoArterialDiastolica { get; set; }

        [Required(ErrorMessage = "A frequência cardíaca é obrigatória.")]
        public required int FrequenciaCardiaca { get; set; }

        [Required(ErrorMessage = "A frequência respiratória é obrigatória.")]
        public required int FrequenciaRespiratoria { get; set; }

        [Required(ErrorMessage = "A temperatura é obrigatória.")]
        public required decimal Temperatura { get; set; }

        [Required(ErrorMessage = "A saturação de oxigênio é obrigatória.")]
        public required int SpO2 { get; set; }

        [Required(ErrorMessage = "A escala de dor é obrigatória.")]
        [Range(0, 10, ErrorMessage = "A escala de dor deve ser de 0 a 10.")]
        public required int EscalaDor { get; set; }

        public int? Glicemia { get; set; }

        public decimal? Peso { get; set; }
    }
}
