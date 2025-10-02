using IntegracionLiquidaciones.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace IntegracionLiquidaciones.Infrastructure.Persistence.Configurations
{
    public class ResumenAsistenciaConfiguration : IEntityTypeConfiguration<ResumenAsistencia>
    {
        public void Configure(EntityTypeBuilder<ResumenAsistencia> builder)
        {
            builder.ToTable("ResumenAsistencia", "dbo");

            builder.HasKey(x => x.IdResumenAsistencia)
                   .HasName("PK_ResumenAsistencia");

            builder.Property(x => x.IdResumenAsistencia)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.RutEmpleado)
                   .HasMaxLength(12)
                   .IsRequired();

            builder.Property(x => x.NombresEmpleado)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.ApellidosEmpleado)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.Periodo)
                   .HasMaxLength(7) // YYYY-MM
                   .IsRequired();

            builder.Property(x => x.SalarioBase)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(x => x.HorasNormales)
                   .HasDefaultValue(0)
                   .IsRequired();

            builder.Property(x => x.HorasExtras)
                   .HasDefaultValue(0)
                   .IsRequired();

            builder.Property(x => x.Inasistencias)
                   .HasDefaultValue(0)
                   .IsRequired();

            builder.Property(x => x.DiasLicencia)
                   .HasDefaultValue(0)
                   .IsRequired();

            builder.Property(x => x.Estado)
                   .HasMaxLength(20)
                   .HasDefaultValue("Pendiente")
                   .IsRequired();

            builder.Property(x => x.FechaRecepcion)
                   .HasDefaultValueSql("SYSUTCDATETIME()")
                   .IsRequired();

            builder.Property(x => x.FechaEnvio)
                   .IsRequired(false);

            builder.HasIndex(x => new { x.RutEmpleado, x.Periodo })
                   .IsUnique()
                   .HasDatabaseName("UX_ResumenAsistencia_Rut_Periodo");

            builder.HasIndex(x => x.Periodo)
                   .HasDatabaseName("IX_ResumenAsistencia_Periodo");

            builder.HasIndex(x => x.Estado)
                   .HasDatabaseName("IX_ResumenAsistencia_Estado");
        }
    }
}
