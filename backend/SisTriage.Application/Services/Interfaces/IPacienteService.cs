using SisTriage.Application.DTOs.Paciente;

namespace SisTriage.Application.Services.Interfaces
{
    public interface IPacienteService
    {
        Task<PacienteResponse> CriarAsync(CriarPacienteRequest request);
        Task<PacienteResponse?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<PacienteResponse>> ListarAsync();
    }
}
