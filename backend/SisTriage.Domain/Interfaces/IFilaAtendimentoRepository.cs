using SisTriage.Domain.Entities;
using SisTriage.Domain.Enums;

namespace SisTriage.Domain.Interfaces
{
    public interface IFilaAtendimentoRepository : IRepository<FilaAtendimento>
    {
        Task<IEnumerable<FilaAtendimento>> ObterFilaOrdenadaPorPrioridadeAsync();
        Task<FilaAtendimento?> ObterComTriagemEPacienteAsync(Guid id);
        Task<IEnumerable<FilaAtendimento>> ObterPorStatusAsync(StatusFila status);
    }
}
