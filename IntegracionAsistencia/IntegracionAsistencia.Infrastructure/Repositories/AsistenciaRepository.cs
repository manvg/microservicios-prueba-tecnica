using IntegracionAsistencia.Application.Interfaces;
using IntegracionAsistencia.Domain.Entities;
using IntegracionAsistencia.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace IntegracionAsistencia.Infrastructure.Repositories
{
    public class AsistenciaRepository : IAsistenciaRepository
    {
        private readonly AppDbContext _context;

        public AsistenciaRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Registra la asistencia de un empleado
        /// Valida que no exista un registro duplicado para el mismo empleado en la misma fecha
        /// </summary>
        public async Task RegistrarAsistenciaAsync(Asistencia asistencia)
        {
            var existe = await _context.Asistencia
                .AnyAsync(a => a.IdEmpleado == asistencia.IdEmpleado && a.Fecha == asistencia.Fecha);

            if (!existe)
            {
                _context.Asistencia.Add(asistencia);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Obtiene las asistencias de un empleado en un rango de fechas
        /// </summary>
        public async Task<IReadOnlyCollection<Asistencia>> ObtenerPorEmpleadoAsync(int idEmpleado, DateTime desde, DateTime hasta)
        {
            return await _context.Asistencia
                .Where(a => a.IdEmpleado == idEmpleado && a.Fecha >= desde && a.Fecha <= hasta)
                .ToListAsync();
        }

        /// <summary>
        /// Calcula el total de horas trabajadas + extras de un empleado en un rango de fechas
        /// </summary>
        public async Task<decimal> CalcularTotalHorasAsync(int idEmpleado, DateTime desde, DateTime hasta)
        {
            return await _context.Asistencia
                .Where(a => a.IdEmpleado == idEmpleado && a.Fecha >= desde && a.Fecha <= hasta)
                .SumAsync(a => a.HorasTrabajadas + a.HorasExtras);
        }
    }
}
