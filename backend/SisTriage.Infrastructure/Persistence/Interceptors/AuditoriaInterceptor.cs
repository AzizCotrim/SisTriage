using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SisTriage.Domain.Entities;
using SisTriage.Domain.Enums;
using SisTriage.Domain.Interfaces;

namespace SisTriage.Infrastructure.Persistence.Interceptors
{
    /// <summary>
    /// Registra automaticamente toda escrita no banco na trilha de auditoria,
    /// sem exigir que os services se preocupem com isso.
    /// </summary>
    public class AuditoriaInterceptor : SaveChangesInterceptor
    {
        private readonly IUsuarioLogado _usuarioLogado;

        public AuditoriaInterceptor(IUsuarioLogado usuarioLogado)
        {
            _usuarioLogado = usuarioLogado;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            if (eventData.Context != null)
                RegistrarLogs(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            if (eventData.Context != null)
                RegistrarLogs(eventData.Context);

            return base.SavingChanges(eventData, result);
        }

        private void RegistrarLogs(DbContext context)
        {
            var entradas = context.ChangeTracker.Entries()
                .Where(e => e.Entity is not LogAuditoria
                            && !e.Metadata.IsOwned()
                            && (e.State == EntityState.Added
                                || e.State == EntityState.Modified
                                || e.State == EntityState.Deleted))
                .ToList();

            if (entradas.Count == 0)
                return;

            var logs = entradas.Select(entrada => new LogAuditoria
            {
                UsuarioId = _usuarioLogado.Id,
                UsuarioEmail = _usuarioLogado.Email,
                Acao = entrada.State switch
                {
                    EntityState.Added => AcaoAuditoria.Criacao,
                    EntityState.Modified => AcaoAuditoria.Alteracao,
                    _ => AcaoAuditoria.Exclusao
                },
                Entidade = entrada.Entity.GetType().Name,
                RegistroId = ObterChavePrimaria(entrada),
                DataHora = DateTime.UtcNow
            }).ToList();

            context.Set<LogAuditoria>().AddRange(logs);
        }

        private static string? ObterChavePrimaria(EntityEntry entrada)
        {
            var propriedade = entrada.Metadata.FindPrimaryKey()?.Properties.FirstOrDefault();

            return propriedade == null
                ? null
                : entrada.Property(propriedade.Name).CurrentValue?.ToString();
        }
    }
}
