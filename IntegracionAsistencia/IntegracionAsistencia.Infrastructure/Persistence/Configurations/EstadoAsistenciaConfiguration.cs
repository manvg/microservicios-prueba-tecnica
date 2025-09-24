using IntegracionAsistencia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntegracionAsistencia.Infrastructure.Persistence.Configurations
{
    public class EstadoAsistenciaConfiguration : IEntityTypeConfiguration<EstadoAsistencia>
    {
        public void Configure(EntityTypeBuilder<EstadoAsistencia> builder)
        {
            builder.ToTable("EstadoAsistencia");

            builder.HasKey(e => e.IdEstadoAsistencia);

            builder.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(30);
        }
    }
}
