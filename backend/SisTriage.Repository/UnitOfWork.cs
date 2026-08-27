using SisTriage.Domain.Entities;
using SisTriage.Domain.Interfaces;
using SisTriage.Infrastructure.Persistence;
using SisTriage.Repository.Repositories;

namespace SisTriage.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IUsuarioRepository Usuarios { get; }

        public IPacienteRepository Pacientes { get; }

        public ITriagemRepository Triagens { get; }

        public IFilaAtendimentoRepository Filas { get; }

        public ILogAuditoriaRepository LogsAuditoria { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Usuarios = new UsuarioRepository(context);
            Pacientes = new PacienteRepository(context);
            Triagens = new TriagemRepository(context);
            Filas = new FilaAtendimentoRepository(context);
            LogsAuditoria = new LogAuditoriaRepository(context);
        }

        public async Task<int> SalvarAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
