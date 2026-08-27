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

        public async Task<(IEnumerable<Usuario> Itens, int Total)> ObterPaginadoAsync(int pagina, int tamanhoPagina)
        {
            var total = await _dbSet.CountAsync();

            var itens = await _dbSet
                .OrderBy(u => u.Nome)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();

            return (itens, total);
        }
    }
}
