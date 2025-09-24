using IntegracionAsistencia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntegracionAsistencia.Infrastructure.Persistence.Configurations
{
    public class TipoJornadaConfiguration : IEntityTypeConfiguration<TipoJornada>
    {
        public void Configure(EntityTypeBuilder<TipoJornada> builder)
        {
            builder.ToTable("TipoJornada");

            builder.HasKey(t => t.IdTipoJornada);

            builder.Property(t => t.Nombre)
                   .IsRequired()
                   .HasMaxLength(50);
        }
    }
}
