using IntegracionLiquidaciones.Domain.Entities;
using IntegracionLiquidaciones.Domain.Interfaces;
using IntegracionLiquidaciones.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace IntegracionLiquidaciones.Infrastructure.Repositories
{
    public class ResumenAsistenciaRepository : IResumenAsistenciaRepository
    {
        private readonly AppDbContext _context;

        public ResumenAsistenciaRepository(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task UpsertAsync(ResumenAsistencia entity, CancellationToken ct = default)
        {
            var existing = await _context.ResumenAsistencia
                .FirstOrDefaultAsync(x => x.RutEmpleado == entity.RutEmpleado && x.Periodo == entity.Periodo, ct);

            if (existing is null)
            {
                await _context.ResumenAsistencia.AddAsync(entity, ct);
            }
            else
            {
                existing.HorasNormales = entity.HorasNormales;
                existing.HorasExtras = entity.HorasExtras;
                existing.Inasistencias = entity.Inasistencias;
                existing.FechaRecepcion = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync(ct);
        }

        public Task<List<ResumenAsistencia>> GetByPeriodoAsync(string periodo, CancellationToken ct = default)
        {
            return _context.ResumenAsistencia
                    .AsNoTracking()
                    .Where(x => x.Periodo == periodo)
                    .OrderBy(x => x.RutEmpleado)
                    .ToListAsync(ct);
        }

    }
}
