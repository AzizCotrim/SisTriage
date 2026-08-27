using SisTriage.Domain.Entities;

namespace SisTriage.Domain.Interfaces
{
    public interface IPacienteRepository : IRepository<Paciente>
    {
        Task<Paciente?> ObterPorCpfAsync(string cpf);
        Task<(IEnumerable<Paciente> Itens, int Total)> ObterPaginadoAsync(int pagina, int tamanhoPagina);
    }
}
