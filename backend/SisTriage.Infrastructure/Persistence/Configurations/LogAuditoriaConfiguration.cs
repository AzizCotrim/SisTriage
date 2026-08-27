using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisTriage.Domain.Entities;

namespace SisTriage.Infrastructure.Persistence.Configurations
{
    public class LogAuditoriaConfiguration : IEntityTypeConfiguration<LogAuditoria>
    {
        public void Configure(EntityTypeBuilder<LogAuditoria> builder)
        {
            builder.ToTable("LogsAuditoria");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.UsuarioId);

            builder.Property(l => l.UsuarioEmail)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(l => l.Acao)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(10);

            builder.Property(l => l.Entidade)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(l => l.RegistroId)
                .HasMaxLength(50);

            builder.Property(l => l.DataHora)
                .IsRequired();

            builder.HasIndex(l => l.DataHora);
        }
    }
}
