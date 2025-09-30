using IntegracionAsistencia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracionAsistencia.Domain.Interfaces
{
    public interface IAsistenciaRepository
    {
        /// <summary>
        /// Registra la asistencia de un empleado
        /// Valida que no exista un registro duplicado para el mismo empleado en la misma fecha
        /// </summary>
        Task RegistrarAsistenciaAsync(Asistencia asistencia);

        /// <summary>
        /// Obtiene las asistencias de un empleado en un rango de fechas
        /// </summary>
        Task<IReadOnlyCollection<Asistencia>> ObtenerPorEmpleadoAsync(int idEmpleado, DateTime desde, DateTime hasta);

        /// <summary>
        /// Calcula el total de horas trabajadas + extras de un empleado en un rango de fechas
        /// </summary>
        Task<decimal> CalcularTotalHorasAsync(int idEmpleado, DateTime desde, DateTime hasta);

        Task<IReadOnlyCollection<Asistencia>> ObtenerPorPeriodoAsync(int idEmpresa, int? idEmpleado, DateTime fechDesde, DateTime fechaHasta);

        /// <summary>
        /// Obtiene los pares (IdEmpleado, Fecha) de asistencias existentes para las fechas especificadas.
        /// </summary>
        Task<List<(int IdEmpleado, DateTime Fecha)>> ObtenerExistentesPorFechasAsync(List<DateTime> fechas);

        /// <summary>
        /// Agrega un rango de asistencias a la base de datos de forma masiva.
        /// </summary>
        Task AgregarRangoAsync(List<Asistencia> asistencias);
    }
}
