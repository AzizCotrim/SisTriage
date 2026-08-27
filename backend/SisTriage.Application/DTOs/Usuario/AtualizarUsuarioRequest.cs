using SisTriage.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SisTriage.Application.DTOs.Usuario
{
    public class AtualizarUsuarioRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(255, ErrorMessage = "O nome deve ter no máximo 255 caracteres.")]
        public required string Nome { get; set; }

        [EnumDataType(typeof(Perfil), ErrorMessage = "Perfil inválido.")]
        public required Perfil Perfil { get; set; }

        public required bool Ativo { get; set; }
    }
}
