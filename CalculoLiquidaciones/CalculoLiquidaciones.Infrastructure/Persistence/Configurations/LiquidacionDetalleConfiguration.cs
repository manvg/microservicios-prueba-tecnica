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
    public class LiquidacionDetalleConfiguration : IEntityTypeConfiguration<LiquidacionDetalle>
    {
        public void Configure(EntityTypeBuilder<LiquidacionDetalle> builder) 
        {
            builder.HasKey(e => e.IdDetalle);

            builder.HasIndex(e => e.IdLiquidacion, "IX_LiquidacionDetalle_IdLiquidacion");

            builder.Property(e => e.Concepto)
                .HasMaxLength(100)
                .IsUnicode(false);
            builder.Property(e => e.Monto).HasColumnType("decimal(10, 2)");
            builder.Property(e => e.TipoConcepto)
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.HasOne(d => d.Liquidacion).WithMany(p => p.LiquidacionDetalle)
                .HasForeignKey(d => d.IdLiquidacion)
                .HasConstraintName("FK_LiquidacionDetalle_Liquidacion");
        }
    }
}
