using System.ComponentModel.DataAnnotations;

namespace SisTriage.Application.DTOs.Triagem
{
    public class CriarTriagemRequest
    {
        public required Guid PacienteId { get; set; }
        public required Guid UsuarioId { get; set; }

        [Required(ErrorMessage = "A queixa principal é obrigatória.")]
        [StringLength(500, ErrorMessage = "A queixa principal deve ter no máximo 500 caracteres.")]
        public required string QueixaPrincipal { get; set; }
    }
}
