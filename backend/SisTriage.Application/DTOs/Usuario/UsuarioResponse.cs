using SisTriage.Domain.Enums;

namespace SisTriage.Application.DTOs.Usuario
{
    public class UsuarioResponse
    {
        public Guid Id { get; set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public Perfil Perfil { get; set; }
        public bool Ativo { get; set; }
    }
}
