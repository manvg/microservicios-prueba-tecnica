using IntegracionAsistencia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntegracionAsistencia.Infrastructure.Persistence.Configurations
{
    public class ResumenAsistenciaConfiguration : IEntityTypeConfiguration<ResumenAsistencia>
    {
        public void Configure(EntityTypeBuilder<ResumenAsistencia> builder)
        {
            builder.ToTable("ResumenAsistencia");

            builder.HasKey(e => e.IdResumenAsistencia);

            builder.Property(e => e.IdEmpresa)
                    .IsRequired();

            builder.Property(e => e.IdEmpleado)
                   .IsRequired();

            builder.Property(e => e.IdTipoNomina)
                   .IsRequired();

            builder.Property(e => e.FechaDesde)
                   .IsRequired();

            builder.Property(e => e.FechaHasta)
                   .IsRequired();

            builder.Property(e => e.HorasNormales)
                   .HasColumnType("decimal(10,2)")
                   .IsRequired();

            builder.Property(e => e.HorasExtras)
                   .HasColumnType("decimal(10,2)")
                   .IsRequired();

            builder.Property(e => e.Inasistencias)
                   .IsRequired();

            builder.Property(e => e.Licencias)
                   .IsRequired();

            builder.Property(e => e.DiasLaborables)
                   .IsRequired();

            builder.Property(e => e.DiasAsistidos)
                   .IsRequired();

            builder.Property(e => e.IdCorrelacion);

            builder.Property(e => e.FechaGeneracion)
                   .HasColumnType("datetime")
                   .HasDefaultValueSql("(getdate())");

            builder.HasIndex(e => new
            {
                e.IdEmpresa,
                e.IdEmpleado,
                e.IdTipoNomina,
                e.FechaDesde,
                e.FechaHasta
            })
            .IsUnique()
            .HasDatabaseName("UX_ResumenAsistencia_Periodo");

            builder.HasOne(e => e.Empresa)
                .WithMany(emp => emp.ResumenesAsistencia)
                .HasForeignKey(e => e.IdEmpresa)
                .HasConstraintName("FK_ResumenAsistencia_Empresa")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Empleado)
                .WithMany(emp => emp.ResumenesAsistencia)
                .HasForeignKey(e => e.IdEmpleado)
                .HasConstraintName("FK_ResumenAsistencia_Empleado")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.TipoNomina)
                .WithMany(tn => tn.ResumenesAsistencia)
                .HasForeignKey(e => e.IdTipoNomina)
                .HasConstraintName("FK_ResumenAsistencia_TipoNomina")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}