using CalculoLiquidaciones.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CalculoLiquidaciones.Infrastructure.Persistence.Contexts;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Empleado> Empleado { get; set; }

    public virtual DbSet<Liquidacion> Liquidacion { get; set; }

    public virtual DbSet<LiquidacionDetalle> LiquidacionDetalle { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Aplicar configuracioens desde Infrastructure.Persistence.Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
