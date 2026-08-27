using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisTriage.Domain.Entities;
using SisTriage.Domain.Enums;

namespace SisTriage.Infrastructure.Persistence.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Nome)
                .IsRequired();

            builder.Property(u => u.Email)
                .IsRequired();

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.SenhaHash)
                .IsRequired()
                .HasMaxLength(60);

            builder.Property(u => u.Perfil)
                .HasConversion<string>()
                .HasMaxLength(13);

            builder.Property(u => u.Ativo)
                .IsRequired();
        }
    }
}
