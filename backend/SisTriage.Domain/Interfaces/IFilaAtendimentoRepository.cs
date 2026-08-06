using SisTriage.Domain.Entities;

namespace SisTriage.Domain.Interfaces
{
    public interface IFilaAtendimentoRepository : IRepository<FilaAtendimento>
    {
        Task<IEnumerable<FilaAtendimento>> ObterFilaOrdenadaPorPrioridadeAsync();
        Task<FilaAtendimento?> ObterComTriagemEPacienteAsync(Guid id);
    }
}
