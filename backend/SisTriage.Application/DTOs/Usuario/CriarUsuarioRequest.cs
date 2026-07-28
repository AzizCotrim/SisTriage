using SisTriage.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SisTriage.Application.DTOs.Usuario
{
    public class CriarUsuarioRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(255, ErrorMessage = "O nome deve ter no máximo 255 caracteres.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
        public required string Senha { get; set; }

        [EnumDataType(typeof(Perfil), ErrorMessage = "Perfil inválido.")]
        public Perfil Perfil { get; set; }
    }
}
