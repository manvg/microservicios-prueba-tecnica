using IntegracionAsistencia.Domain.Entities;

namespace IntegracionAsistencia.Application.Interfaces
{
    public interface IAsistenciaService
    {
        Task RegistrarAsistenciaAsync(Asistencia asistencia);
        Task<IReadOnlyCollection<Asistencia>> ObtenerPorEmpleadoAsync(int idEmpleado, DateTime desde, DateTime hasta);
        Task<decimal> CalcularTotalHorasAsync(int idEmpleado, DateTime desde, DateTime hasta);
    }
}
