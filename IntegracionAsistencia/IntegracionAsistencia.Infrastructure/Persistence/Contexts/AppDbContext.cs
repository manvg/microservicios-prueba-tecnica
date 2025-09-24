using IntegracionAsistencia.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IntegracionAsistencia.Infrastructure.Persistence.Contexts;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Asistencia> Asistencia { get; set; }

    public virtual DbSet<Empleado> Empleado { get; set; }

    public virtual DbSet<Empresa> Empresa { get; set; }

    public virtual DbSet<EstadoAsistencia> EstadoAsistencia { get; set; }

    public virtual DbSet<TipoJornada> TipoJornada { get; set; }

    public virtual DbSet<TipoNomina> TipoNomina { get; set; }

    public virtual DbSet<TipoOrigenDato> TipoOrigenDato { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Aplicar configuracioens desde Infrastructure.Persistence.Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
