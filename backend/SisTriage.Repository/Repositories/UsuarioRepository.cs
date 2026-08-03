using Microsoft.EntityFrameworkCore;
using SisTriage.Domain.Entities;
using SisTriage.Domain.Interfaces;
using SisTriage.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace SisTriage.Repository.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Usuario?> ObterPorEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
