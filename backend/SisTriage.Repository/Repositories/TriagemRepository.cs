using Microsoft.EntityFrameworkCore;
using SisTriage.Domain.Entities;
using SisTriage.Domain.Interfaces;
using SisTriage.Infrastructure.Persistence;

namespace SisTriage.Repository.Repositories
{
    public class TriagemRepository : Repository<Triagem>, ITriagemRepository
    {
        public TriagemRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Triagem?> ObterComPacienteEUsuarioAsync(Guid id)
        {
            return await _dbSet
                .Include(t => t.Paciente)
                .Include(t => t.Usuario)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
