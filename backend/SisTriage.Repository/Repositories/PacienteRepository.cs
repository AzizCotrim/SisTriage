using Microsoft.EntityFrameworkCore;
using SisTriage.Domain.Entities;
using SisTriage.Domain.Interfaces;
using SisTriage.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace SisTriage.Repository.Repositories
{
    public class PacienteRepository : Repository<Paciente>, IPacienteRepository
    {
        public PacienteRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Paciente?> ObterPorCpfAsync(string cpf)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.CPF == cpf);
        }

        public async Task<(IEnumerable<Paciente> Itens, int Total)> ObterPaginadoAsync(int pagina, int tamanhoPagina)
        {
            var total = await _dbSet.CountAsync();

            var itens = await _dbSet
                .OrderBy(p => p.Nome)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .ToListAsync();

            return (itens, total);
        }
    }
}
