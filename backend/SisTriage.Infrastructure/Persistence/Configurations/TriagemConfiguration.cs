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

            builder.OwnsOne(t => t.SinaisVitais, sv =>
            {
                sv.Property(s => s.PressaoArterialSistolica)
                    .IsRequired();

                sv.Property(s => s.PressaoArterialDiastolica)
                    .IsRequired();

                sv.Property(s => s.FrequenciaCardiaca)
                    .IsRequired();

                sv.Property(s => s.FrequenciaRespiratoria)
                    .IsRequired();

                sv.Property(s => s.Temperatura)
                    .IsRequired()
                    .HasColumnType("numeric(4,1)");

                sv.Property(s => s.SpO2)
                    .IsRequired();

                sv.Property(s => s.EscalaDor)
                    .IsRequired();

                sv.Property(s => s.Glicemia);

                sv.Property(s => s.Peso)
                    .HasColumnType("numeric(5,2)");
            });
        }
    }
}
