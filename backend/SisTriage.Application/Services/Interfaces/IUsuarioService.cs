using SisTriage.Application.DTOs.Usuario;

namespace SisTriage.Application.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioResponse> CriarAsync(CriarUsuarioRequest request);
        Task<string> AutenticarAsync(LoginRequest request);
    }
}
