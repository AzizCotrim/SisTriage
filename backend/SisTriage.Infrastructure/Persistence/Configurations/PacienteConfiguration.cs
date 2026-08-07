using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisTriage.Domain.Entities;

namespace SisTriage.Infrastructure.Persistence.Configurations
{
    public class PacienteConfiguration : IEntityTypeConfiguration<Paciente>
    {
        public void Configure(EntityTypeBuilder<Paciente> builder)
        {
            builder.ToTable("Pacientes");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome)
                .IsRequired();

            builder.Property(p => p.CPF)
                .IsRequired()
                .HasMaxLength(11);

            builder.HasIndex(p => p.CPF)
                .IsUnique();

            builder.Property(p => p.DataNascimento)
                .HasColumnType("date");

            builder.Property(p => p.Sexo)
                .IsRequired()
                .HasMaxLength(9)
                .HasConversion<string>();

            builder.Property(p => p.Telefone)
                .IsRequired()
                .HasMaxLength(13);

            builder.Property(p => p.Alergias)
                .HasMaxLength(500);

        }
    }
}
