using System.ComponentModel.DataAnnotations;

namespace SisTriage.Application.DTOs.Paciente
{
    public class CriarPacienteRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(255, ErrorMessage = "O nome deve ter no máximo 255 caracteres.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve ter 11 dígitos.")]
        public required string CPF { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }

        [Required]
        [StringLength(9)]
        public required string Sexo { get; set; }

        [Required]
        [StringLength(13)]
        public required string Telefone { get; set; }
    }
}
