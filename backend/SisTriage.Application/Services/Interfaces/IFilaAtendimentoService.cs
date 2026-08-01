using SisTriage.Application.DTOs.FilaAtendimento;

namespace SisTriage.Application.Services.Interfaces
{
    public interface IFilaAtendimentoService
    {
        Task<IEnumerable<FilaAtendimentoResponse>> ObterFilaOrdenadaAsync();
        Task<FilaAtendimentoResponse?> ChamarProximoAsync();
    }
}
