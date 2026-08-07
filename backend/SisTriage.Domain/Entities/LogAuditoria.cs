using SisTriage.Domain.Enums;

namespace SisTriage.Domain.Entities
{
    /// <summary>
    /// Registro das operações de tratamento de dados pessoais (LGPD, art. 37).
    /// Guarda o e-mail do usuário em vez de uma FK para Usuario de propósito:
    /// o log precisa continuar íntegro mesmo que o usuário seja removido.
    /// </summary>
    public class LogAuditoria
    {
        public Guid Id { get; private set; }
        public Guid? UsuarioId { get; set; }
        public required string UsuarioEmail { get; set; }
        public required AcaoAuditoria Acao { get; set; }
        public required string Entidade { get; set; }
        public string? RegistroId { get; set; }
        public required DateTime DataHora { get; set; }

        public LogAuditoria()
        {
            Id = Guid.NewGuid();
        }
    }
}
