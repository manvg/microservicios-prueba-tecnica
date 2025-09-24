using IntegracionAsistencia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntegracionAsistencia.Infrastructure.Persistence.Configurations
{
    public class TipoOrigenDatoConfiguration : IEntityTypeConfiguration<TipoOrigenDato>
    {
        public void Configure(EntityTypeBuilder<TipoOrigenDato> builder)
        {
            builder.ToTable("TipoOrigenDato");

            builder.HasKey(t => t.IdTipoOrigenDato);

            builder.Property(t => t.Nombre)
                .IsRequired()
                .HasMaxLength(20);
        }
    }
}
