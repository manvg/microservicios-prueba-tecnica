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

            builder.HasOne(e => e.Empleado)
                            .WithMany(emp => emp.Asistencias)
                            .HasForeignKey(e => e.IdEmpleado)
                            .HasConstraintName("FK_Asistencia_Empleado")
                            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.TipoJornada)
                .WithMany(tj => tj.Asistencias)
                .HasForeignKey(e => e.IdTipoJornada)
                .HasConstraintName("FK_Asistencia_TipoJornada")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.EstadoAsistencia)
                .WithMany(ea => ea.Asistencias)
                .HasForeignKey(e => e.IdEstadoAsistencia)
                .HasConstraintName("FK_Asistencia_EstadoAsistencia")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.TipoOrigenDato)
                .WithMany(tod => tod.Asistencias)
                .HasForeignKey(e => e.IdTipoOrigenDato)
                .HasConstraintName("FK_Asistencia_TipoOrigenDato")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => new { e.IdEmpleado, e.Fecha });
        }
    }
}