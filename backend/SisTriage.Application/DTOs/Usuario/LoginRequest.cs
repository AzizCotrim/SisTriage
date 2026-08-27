using System.ComponentModel.DataAnnotations;

namespace SisTriage.Application.DTOs.Usuario
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        public required string Senha { get; set; }
    }
}
