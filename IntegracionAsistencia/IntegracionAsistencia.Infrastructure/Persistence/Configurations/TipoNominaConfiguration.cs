using IntegracionAsistencia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntegracionAsistencia.Infrastructure.Persistence.Configurations
{
    public class TipoNominaConfiguration : IEntityTypeConfiguration<TipoNomina>
    {
        public void Configure(EntityTypeBuilder<TipoNomina> builder)
        {
            builder.ToTable("TipoNomina");

            builder.HasKey(t => t.IdTipoNomina);

            builder.Property(t => t.Nombre)
                .IsRequired()
                .HasMaxLength(20);
        }
    }
}
