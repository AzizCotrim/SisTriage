using SisTriage.Domain.Entities;

namespace SisTriage.Domain.Interfaces
{
    public interface ILogAuditoriaRepository : IRepository<LogAuditoria>
    {
        Task<IEnumerable<LogAuditoria>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim);
    }
}
