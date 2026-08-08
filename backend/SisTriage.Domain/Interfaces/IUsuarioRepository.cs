using SisTriage.Domain.Entities;

namespace SisTriage.Domain.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario?> ObterPorEmailAsync(string email);
        Task<(IEnumerable<Usuario> Itens, int Total)> ObterPaginadoAsync(int pagina, int tamanhoPagina);
    }
}