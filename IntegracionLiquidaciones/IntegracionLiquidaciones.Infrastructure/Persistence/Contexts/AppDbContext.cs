using IntegracionLiquidaciones.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IntegracionLiquidaciones.Infrastructure.Persistence.Contexts;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ResumenAsistencia> ResumenAsistencia { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Aplicar configuracioens desde Infrastructure.Persistence.Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
