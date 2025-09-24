using IntegracionAsistencia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntegracionAsistencia.Infrastructure.Persistence.Configurations
{
    public class AsistenciaConfiguration : IEntityTypeConfiguration<Asistencia>
    {
        public void Configure(EntityTypeBuilder<Asistencia> builder)
        {
            builder.ToTable("Asistencia");

            builder.HasKey(a => a.IdAsistencia);

            builder.Property(a => a.Fecha)
                   .IsRequired();

            builder.Property(a => a.HorasTrabajadas)
                   .HasColumnType("decimal(5,2)");

            builder.Property(a => a.HorasExtras)
                   .HasColumnType("decimal(5,2)");

            builder.HasOne(a => a.Empleado)
                            .WithMany(e => e.Asistencias)
                            .HasForeignKey(a => a.IdEmpleado)
                            .HasConstraintName("FK_Asistencia_Empleado");

            builder.HasOne(a => a.TipoJornada)
                   .WithMany(t => t.Asistencias)
                   .HasForeignKey(a => a.IdTipoJornada)
                   .HasConstraintName("FK_Asistencia_TipoJornada");

            builder.HasOne(a => a.EstadoAsistencia)
                   .WithMany(ea => ea.Asistencias)
                   .HasForeignKey(a => a.IdEstadoAsistencia)
                   .HasConstraintName("FK_Asistencia_EstadoAsistencia");

            builder.HasOne(a => a.TipoOrigenDato)
                   .WithMany(to => to.Asistencias)
                   .HasForeignKey(a => a.IdTipoOrigenDato)
                   .HasConstraintName("FK_Asistencia_TipoOrigenDato");
        }
    }
}