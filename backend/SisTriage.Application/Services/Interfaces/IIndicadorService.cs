using SisTriage.Application.DTOs.Indicador;

namespace SisTriage.Application.Services.Interfaces
{
    public interface IIndicadorService
    {
        Task<IndicadoresResponse> ObterAsync(IndicadoresRequest request);
    }
}
