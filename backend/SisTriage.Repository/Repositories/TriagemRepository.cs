using SisTriage.Domain.Entities;
using SisTriage.Domain.Interfaces;
using SisTriage.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace SisTriage.Repository.Repositories
{
    public class TriagemRepository : Repository<Triagem>, ITriagemRepository
    {
        public TriagemRepository(AppDbContext context) : base(context)
        {
        }
    }
}
