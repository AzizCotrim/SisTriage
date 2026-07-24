using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisTriage.Domain.Entities;

namespace SisTriage.Infrastructure.Persistence.Configurations
{
    public class FilaAtendimentoConfiguration : IEntityTypeConfiguration<FilaAtendimento>
    {
        public void Configure(EntityTypeBuilder<FilaAtendimento> builder)
        {
            builder.ToTable("FilaAtendimentos");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.TriagemId)
                .IsRequired();

            builder.HasOne(f => f.Triagem)
                .WithOne()
                .HasForeignKey<FilaAtendimento>(t => t.TriagemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(f => f.DataEntrada)
                .IsRequired();

            builder.Property(f => f.DataSaida);

            builder.Property(f => f.Status)
                .IsRequired()
                .HasConversion<string>();
        }
    }
}
