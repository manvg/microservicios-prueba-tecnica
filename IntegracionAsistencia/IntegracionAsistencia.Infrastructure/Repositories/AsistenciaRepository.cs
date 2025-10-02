using IntegracionAsistencia.Domain.Entities;
using IntegracionAsistencia.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using IntegracionAsistencia.Domain.Interfaces;

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
                .Where(a => a.IdEmpleado == idEmpleado && a.Fecha.Date >= desde && a.Fecha.Date <= hasta)
                .ToListAsync();
        }

        /// <summary>
        /// Calcula el total de horas trabajadas + extras de un empleado en un rango de fechas
        /// </summary>
        public async Task<decimal> CalcularTotalHorasAsync(int idEmpleado, DateTime desde, DateTime hasta)
        {
            return await _context.Asistencia
                .Where(a => a.IdEmpleado == idEmpleado && a.Fecha.Date >= desde && a.Fecha.Date <= hasta)
                .SumAsync(a => a.HorasTrabajadas + a.HorasExtras);
        }

        /// <summary>
        /// Obtiene las asistencias de una empresa y empleado en un período específico
        /// Si idEmpleado es null, devuelve todos los empleados de la empresa
        /// </summary>
        public async Task<IReadOnlyCollection<Asistencia>> ObtenerPorPeriodoAsync(int idEmpresa, int? idEmpleado, DateTime fechDesde,DateTime fechaHasta)
        {
            var query = _context.Asistencia
                .Where(a => a.Empleado.IdEmpresa == idEmpresa && a.Fecha.Date >= fechDesde && a.Fecha.Date <= fechaHasta);

            if (idEmpleado.HasValue && idEmpleado.Value > 0)
                query = query.Where(a => a.IdEmpleado == idEmpleado.Value);

            return await query.ToListAsync();
        }

        /// <summary>
        /// Obtiene los pares (IdEmpleado, Fecha) de asistencias existentes para las fechas especificadas.
        /// Optimizado para detectar duplicados en carga masiva.
        /// </summary>
        public async Task<List<(int IdEmpleado, DateTime Fecha)>> ObtenerExistentesPorFechasAsync(List<DateTime> fechas)
        {
            var fechasSinHora = fechas.Select(f => f.Date).Distinct().ToList();

            return await _context.Asistencia
                .Where(a => fechasSinHora.Contains(a.Fecha.Date))
                .Select(a => new ValueTuple<int, DateTime>(a.IdEmpleado, a.Fecha.Date))
                .ToListAsync();
        }

        /// <summary>
        /// Agrega un rango de asistencias a la base de datos de forma masiva.
        /// Operación optimizada para inserción de múltiples registros.
        /// </summary>
        public async Task AgregarRangoAsync(List<Asistencia> asistencias)
        {
            if (asistencias == null || !asistencias.Any())
            {
                return;
            }

            await _context.Asistencia.AddRangeAsync(asistencias);
            await _context.SaveChangesAsync();
        }
    }
}
