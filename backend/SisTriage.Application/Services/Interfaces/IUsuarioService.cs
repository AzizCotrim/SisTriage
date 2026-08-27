using SisTriage.Application.DTOs.Common;
using SisTriage.Application.DTOs.Usuario;

namespace SisTriage.Application.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioResponse> CriarAsync(CriarUsuarioRequest request);
        Task<string> AutenticarAsync(LoginRequest request);
        Task<ResultadoPaginado<UsuarioResponse>> ListarAsync(PaginacaoRequest paginacao);
        Task<UsuarioResponse?> ObterPorIdAsync(Guid id);
        Task<UsuarioResponse> AtualizarAsync(Guid id, AtualizarUsuarioRequest request);
    }
}
