using Microsoft.EntityFrameworkCore;
using SisTriage.Domain.Entities;
using SisTriage.Domain.Interfaces;
using SisTriage.Infrastructure.Persistence;
using SisTriage.Domain.Enums;

namespace SisTriage.Repository.Repositories
{
    public class FilaAtendimentoRepository : Repository<FilaAtendimento>, IFilaAtendimentoRepository
    {
        public FilaAtendimentoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<FilaAtendimento>> ObterFilaOrdenadaPorPrioridadeAsync()
        {
            var fila = await _dbSet
                .Include(f => f.Triagem)
                .Where(f => f.Status == StatusFila.Aguardando)
                .ToListAsync();

            return fila
                .OrderBy(f => f.Triagem.Classificacao.TempoMaximoEsperaMinutos())
                .ThenBy(f => f.DataEntrada);
        }
    }
}
