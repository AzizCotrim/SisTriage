using System.ComponentModel.DataAnnotations;

namespace SisTriage.Application.DTOs.Paciente
{
    public class AtualizarPacienteRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(255, ErrorMessage = "O nome deve ter no máximo 255 caracteres.")]
        public required string Nome { get; set; }

        [Required]
        [StringLength(9)]
        public required string Sexo { get; set; }

        [Required]
        [StringLength(13)]
        public required string Telefone { get; set; }

        [StringLength(500, ErrorMessage = "As alergias devem ter no máximo 500 caracteres.")]
        public string? Alergias { get; set; }
    }
}
