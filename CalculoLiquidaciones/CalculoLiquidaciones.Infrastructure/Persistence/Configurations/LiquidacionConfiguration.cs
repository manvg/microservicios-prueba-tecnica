using CalculoLiquidaciones.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoLiquidaciones.Infrastructure.Persistence.Configurations
{
    public class LiquidacionConfiguration : IEntityTypeConfiguration<Liquidacion>
    {
        public void Configure(EntityTypeBuilder<Liquidacion> builder)
        {
            builder.HasKey(e => e.IdLiquidacion);

            builder.HasIndex(e => e.FechaGeneracion, "IX_Liquidacion_FechaGeneracion");

            builder.HasIndex(e => e.IdEmpleado, "IX_Liquidacion_IdEmpleado");

            builder.HasIndex(e => e.Periodo, "IX_Liquidacion_Periodo");

            builder.HasIndex(e => new { e.RutEmpleado, e.Periodo }, "UQ_Liquidacion_Rut_Periodo").IsUnique();

            builder.Property(e => e.FechaGeneracion).HasColumnType("datetime");
            builder.Property(e => e.Periodo)
                .HasMaxLength(7)
                .IsUnicode(false);
            builder.Property(e => e.RutEmpleado)
                .HasMaxLength(15)
                .IsUnicode(false);
            builder.Property(e => e.SueldoBase).HasColumnType("decimal(10, 2)");
            builder.Property(e => e.TotalDescuentos).HasColumnType("decimal(10, 2)");
            builder.Property(e => e.TotalHorasExtras).HasColumnType("decimal(10, 2)");
            builder.Property(e => e.TotalLiquido).HasColumnType("decimal(10, 2)");

            builder.HasOne(d => d.Empleado).WithMany(p => p.Liquidacion)
                .HasForeignKey(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Liquidacion_Empleado");
        }
    }
}
