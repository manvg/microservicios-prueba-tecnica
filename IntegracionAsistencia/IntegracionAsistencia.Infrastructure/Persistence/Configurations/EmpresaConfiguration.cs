using IntegracionAsistencia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntegracionAsistencia.Infrastructure.Persistence.Configurations
{
    public class EmpresaConfiguration : IEntityTypeConfiguration<Empresa>
    {
        public void Configure(EntityTypeBuilder<Empresa> builder)
        {
            builder.ToTable("Empresa");

            builder.HasKey(e => e.IdEmpresa);

            builder.Property(e => e.RutEmpresa)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false);

            builder.Property(e => e.RazonSocial)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false);

            // FK a catálogo TipoNomina
            builder.Property(e => e.IdTipoNomina)
                .IsRequired();

            builder.HasOne(e => e.TipoNomina)
                .WithMany(t => t.Empresas)
                .HasForeignKey(e => e.IdTipoNomina)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Empresa_TipoNomina");
        }
    }
}
