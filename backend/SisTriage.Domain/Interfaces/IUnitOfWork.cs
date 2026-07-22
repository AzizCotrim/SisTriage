namespace SisTriage.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IUsuarioRepository Usuarios { get; }
        IPacienteRepository Pacientes { get; }
        ITriagemRepository Triagens { get; }
        IFilaAtendimentoRepository Filas { get; }

        Task<int> SalvarAsync();
    }
}
