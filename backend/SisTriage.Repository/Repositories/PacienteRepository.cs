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
    }
}
