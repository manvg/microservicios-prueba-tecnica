using IntegracionAsistencia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntegracionAsistencia.Infrastructure.Persistence.Configurations
{
    public class EmpleadoConfiguration : IEntityTypeConfiguration<Empleado>
    {
        public void Configure(EntityTypeBuilder<Empleado> builder)
        {
            builder.ToTable("Empleado");

            builder.HasKey(e => e.IdEmpleado);

            builder.Property(e => e.Rut)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(e => e.Nombres)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Apellidos)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Email)
                .HasMaxLength(100);

            builder.Property(e => e.Cargo)
                .HasMaxLength(100);

            builder.Property(e => e.Departamento)
                .HasMaxLength(100);

            builder.Property(e => e.FechaIngreso)
                .IsRequired()
                .HasColumnType("date");

            builder.Property(e => e.SalarioBase)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(e => e.HoraEntrada)
                .IsRequired()
                .HasColumnType("time");

            builder.Property(e => e.HoraSalida)
                .IsRequired()
                .HasColumnType("time");

            builder.Property(e => e.HorasSemanales)
                .IsRequired();

            builder.Property(e => e.Activo)
                .IsRequired();

            // Relación con Empresa
            builder.HasOne(e => e.Empresa)
                .WithMany(emp => emp.Empleados)
                .HasForeignKey(e => e.IdEmpresa)
                .HasConstraintName("FK_Empleado_Empresa")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}


