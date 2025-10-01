using CalculoLiquidaciones.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CalculoLiquidaciones.Infrastructure.Persistence.Configurations
{
    public class EmpleadoConfiguration : IEntityTypeConfiguration<Empleado>
    {
        public void Configure(EntityTypeBuilder<Empleado> builder)
        {
            builder.HasKey(e => e.IdEmpleado);

            builder.HasIndex(e => e.Rut, "UQ_Empleado_Rut").IsUnique();

            builder.Property(e => e.Apellidos)
                .HasMaxLength(100)
                .IsUnicode(false);
            builder.Property(e => e.Nombres)
                .HasMaxLength(100)
                .IsUnicode(false);
            builder.Property(e => e.Rut)
                .HasMaxLength(15)
                .IsUnicode(false);
            builder.Property(e => e.SueldoBase).HasColumnType("decimal(12, 2)");
        }
    }
}
