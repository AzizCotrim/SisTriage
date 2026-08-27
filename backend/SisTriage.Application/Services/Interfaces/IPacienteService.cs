using SisTriage.Application.DTOs.Common;
using SisTriage.Application.DTOs.Paciente;

namespace SisTriage.Application.Services.Interfaces
{
    public interface IPacienteService
    {
        Task<PacienteResponse> CriarAsync(CriarPacienteRequest request);
        Task<PacienteResponse?> ObterPorIdAsync(Guid id);
        Task<ResultadoPaginado<PacienteListaResponse>> ListarAsync(PaginacaoRequest paginacao);
        Task<PacienteResponse> AtualizarAsync(Guid id, AtualizarPacienteRequest request);
    }
}
