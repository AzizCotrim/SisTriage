using SisTriage.Application.DTOs.Triagem;

namespace SisTriage.Application.Services.Interfaces
{
    public interface ITriagemService
    {
        Task<TriagemResponse> RegistrarAsync(CriarTriagemRequest request);
        Task<TriagemResponse?> ObterPorIdAsync(Guid id);
    }
}
