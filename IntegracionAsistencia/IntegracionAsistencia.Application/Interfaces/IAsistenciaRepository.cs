using IntegracionAsistencia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracionAsistencia.Application.Interfaces
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
    }
}
