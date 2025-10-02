using CalculoLiquidaciones.Domain.Entities;
using CalculoLiquidaciones.Domain.Interfaces;
using CalculoLiquidaciones.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CalculoLiquidaciones.Infrastructure.Repositories
{
    public class LiquidacionRepository : ILiquidacionRepository
    {
        private readonly AppDbContext _context;

        public LiquidacionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Liquidacion>> ObtenerLiquidacionesAsync()
        {
            return await _context.Liquidacion.Include(l => l.Empleado).Include(l => l.LiquidacionDetalle)
                .ToListAsync();
        }

        public async Task GuardarLiquidacionAsync(Liquidacion liquidacion)
        {
            var existente = await _context.Liquidacion
                .FirstOrDefaultAsync(l =>l.RutEmpleado == liquidacion.RutEmpleado 
                && l.Periodo == liquidacion.Periodo);

            if (existente == null)
            {
                _context.Liquidacion.Add(liquidacion);
            }
            else
            {
                existente.SueldoBase = liquidacion.SueldoBase;
                existente.TotalHorasExtras = liquidacion.TotalHorasExtras;
                existente.TotalDescuentos = liquidacion.TotalDescuentos;
                existente.TotalLiquido = liquidacion.TotalLiquido;
                existente.FechaGeneracion = liquidacion.FechaGeneracion;
                existente.IdCorrelacion = liquidacion.IdCorrelacion;
            }

            await _context.SaveChangesAsync();
        }
    }
}
