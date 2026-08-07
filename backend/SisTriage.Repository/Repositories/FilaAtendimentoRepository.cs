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
                    .ThenInclude(t => t.Paciente)
                .Where(f => f.Status == StatusFila.Aguardando)
                .ToListAsync();

            return fila
                .OrderBy(f => f.Triagem.Classificacao.TempoMaximoEsperaMinutos())
                .ThenBy(f => f.DataEntrada);
        }

        public async Task<FilaAtendimento?> ObterComTriagemEPacienteAsync(Guid id)
        {
            return await _dbSet
                .Include(f => f.Triagem)
                    .ThenInclude(t => t.Paciente)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<FilaAtendimento>> ObterPorStatusAsync(StatusFila status)
        {
            return await _dbSet
                .Include(f => f.Triagem)
                    .ThenInclude(t => t.Paciente)
                .Where(f => f.Status == status)
                .OrderByDescending(f => f.DataEntrada)
                .ToListAsync();
        }
    }
}
