using CalculoLiquidaciones.Domain.Entities;
using CalculoLiquidaciones.Domain.Interfaces;
using CalculoLiquidaciones.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CalculoLiquidaciones.Infrastructure.Repositories
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly AppDbContext _context;

        public EmpleadoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Empleado?> ObtenerPorRutAsync(string rut)
        {
            return await _context.Empleado.FirstOrDefaultAsync(e => e.Rut == rut);
        }

        public async Task<Empleado?> ObtenerPorIdAsync(int idEmpleado)
        {
            return await _context.Empleado.FirstOrDefaultAsync(e => e.IdEmpleado == idEmpleado);
        }

        public async Task<Empleado> AgregarAsync(Empleado empleado)
        {
            _context.Empleado.Add(empleado);
            await _context.SaveChangesAsync();
            return empleado;
        }
    }
}
