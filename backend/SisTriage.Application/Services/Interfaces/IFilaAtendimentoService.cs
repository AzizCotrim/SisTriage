using SisTriage.Application.DTOs.FilaAtendimento;

namespace SisTriage.Application.Services.Interfaces
{
    public interface IFilaAtendimentoService
    {
        Task<IEnumerable<FilaAtendimentoResponse>> ObterFilaOrdenadaAsync();
        Task<FilaAtendimentoResponse?> ChamarProximoAsync();
        Task<FilaAtendimentoResponse> FinalizarAsync(Guid filaAtendimentoId);
        Task<FilaAtendimentoResponse> RegistrarNaoComparecimentoAsync(Guid filaAtendimentoId);
        Task<IEnumerable<FilaAtendimentoResponse>> ObterNaoAtendidosAsync();
    }
}
