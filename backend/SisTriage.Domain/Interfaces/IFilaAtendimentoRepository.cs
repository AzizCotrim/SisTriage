using SisTriage.Domain.Entities;
using SisTriage.Domain.Enums;

namespace SisTriage.Domain.Interfaces
{
    public interface IFilaAtendimentoRepository : IRepository<FilaAtendimento>
    {
        Task<IEnumerable<FilaAtendimento>> ObterFilaOrdenadaPorPrioridadeAsync();
        Task<FilaAtendimento?> ObterComTriagemEPacienteAsync(Guid id);
        Task<(IEnumerable<FilaAtendimento> Itens, int Total)> ObterPorStatusPaginadoAsync(StatusFila status, int pagina, int tamanhoPagina);
        Task<IEnumerable<FilaAtendimento>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim);
    }
}
