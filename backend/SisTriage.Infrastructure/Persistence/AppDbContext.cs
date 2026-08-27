using Microsoft.EntityFrameworkCore;
using SisTriage.Domain.Entities;

namespace SisTriage.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Paciente> Pacientes { get; set; } = null!;
        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Triagem> Triagens { get; set; } = null!;
        public DbSet<FilaAtendimento> FilaAtendimentos { get; set; } = null!;
        public DbSet<LogAuditoria> LogsAuditoria { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}