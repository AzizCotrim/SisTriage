using SisTriage.Domain.Enums;

namespace SisTriage.Domain.Entities
{
    public class Usuario
    {
        public Guid Id { get; private set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string SenhaHash { get; set; }
        public Perfil Perfil { get; set; }
        public bool Ativo { get; set; }

        public Usuario()
        {
            Id = Guid.NewGuid();
        }
    }
}
