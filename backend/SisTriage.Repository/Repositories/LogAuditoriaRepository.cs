using Microsoft.EntityFrameworkCore;
using SisTriage.Domain.Entities;
using SisTriage.Domain.Interfaces;
using SisTriage.Infrastructure.Persistence;

namespace SisTriage.Repository.Repositories
{
    public class LogAuditoriaRepository : Repository<LogAuditoria>, ILogAuditoriaRepository
    {
        public LogAuditoriaRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<LogAuditoria>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim)
        {
            return await _dbSet
                .Where(l => l.DataHora >= inicio && l.DataHora <= fim)
                .OrderByDescending(l => l.DataHora)
                .ToListAsync();
        }
    }
}
