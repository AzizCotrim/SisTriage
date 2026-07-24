using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisTriage.Domain.Entities;

namespace SisTriage.Infrastructure.Persistence.Configurations
{
    public class TriagemConfiguration : IEntityTypeConfiguration<Triagem>
    {
        public void Configure(EntityTypeBuilder<Triagem> builder)
        {
            builder.ToTable("Triagens");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.PacienteId)
                .IsRequired();

            builder.HasOne(t => t.Paciente)
                .WithMany(p => p.Triagens)
                .HasForeignKey(t => t.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(t => t.UsuarioId)
                .IsRequired();

            builder.HasOne(t => t.Usuario)
                .WithMany(u => u.Triagens)
                .HasForeignKey(t => t.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(t => t.DataHora)
                .IsRequired();

            builder.Property(t => t.Classificacao)
                .HasConversion<string>();

            builder.Property(t => t.QueixaPrincipal)
                .IsRequired();

            builder.Property(t => t.Status)
                .IsRequired()
                .HasConversion<string>();
        }
    }
}
