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
            // Inclui quem já foi chamado: o paciente em atendimento precisa continuar
            // visível no painel para que seja possível finalizar ou registrar ausência.
            var fila = await _dbSet
                .Include(f => f.Triagem)
                    .ThenInclude(t => t.Paciente)
                .Where(f => f.Status == StatusFila.Aguardando || f.Status == StatusFila.Chamado)
                .ToListAsync();

            return fila
                .OrderBy(f => f.Status == StatusFila.Chamado ? 0 : 1)
                .ThenBy(f => f.Triagem.Classificacao.TempoMaximoEsperaMinutos())
                .ThenBy(f => f.DataEntrada);
        }

        public async Task<FilaAtendimento?> ObterComTriagemEPacienteAsync(Guid id)
        {
            return await _dbSet
                .Include(f => f.Triagem)
                    .ThenInclude(t => t.Paciente)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<FilaAtendimento>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim)
        {
            return await _dbSet
                .Include(f => f.Triagem)
                .Where(f => f.DataEntrada >= inicio && f.DataEntrada <= fim)
                .ToListAsync();
        }

        public async Task<(IEnumerable<FilaAtendimento> Itens, int Total)> ObterPorStatusPaginadoAsync(StatusFila status, int pagina, int tamanhoPagina)
        {
            var consulta = _dbSet.Where(f => f.Status == status);

            var total = await consulta.CountAsync();

            var itens = await consulta
                .Include(f => f.Triagem)
                    .ThenInclude(t => t.Paciente)
                .OrderByDescending(f => f.DataEntrada)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();

            return (itens, total);
        }
    }
}
