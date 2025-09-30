using IntegracionAsistencia.Application.Interfaces;
using IntegracionAsistencia.Domain.Entities;
using IntegracionAsistencia.Domain.Interfaces;

namespace IntegracionAsistencia.Application.Services
{
    /// <summary>
    /// Servicio de aplicación para la gestión de asistencia.
    /// </summary>
    public class AsistenciaService : IAsistenciaService
    {
        private readonly IAsistenciaRepository _asistenciaRepository;

        public AsistenciaService(IAsistenciaRepository asistenciaRepository)
        {
            _asistenciaRepository = asistenciaRepository;
        }

        /// <summary>
        /// Registra una asistencia, validando duplicados a nivel de repositorio.
        /// </summary>
        public async Task RegistrarAsistenciaAsync(Asistencia asistencia)
        {
            await _asistenciaRepository.RegistrarAsistenciaAsync(asistencia);
        }

        /// <summary>
        /// Obtiene todas las asistencias de un empleado en un rango de fechas.
        /// </summary>
        public async Task<IReadOnlyCollection<Asistencia>> ObtenerPorEmpleadoAsync(int idEmpleado, DateTime desde, DateTime hasta)
        {
            return await _asistenciaRepository.ObtenerPorEmpleadoAsync(idEmpleado, desde, hasta);
        }

        /// <summary>
        /// Calcula el total de horas trabajadas + extras de un empleado en un rango de fechas.
        /// </summary>
        public async Task<decimal> CalcularTotalHorasAsync(int idEmpleado, DateTime desde, DateTime hasta)
        {
            return await _asistenciaRepository.CalcularTotalHorasAsync(idEmpleado, desde, hasta);
        }
    }
}
