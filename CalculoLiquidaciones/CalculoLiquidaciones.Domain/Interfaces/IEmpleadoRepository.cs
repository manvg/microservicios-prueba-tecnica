using CalculoLiquidaciones.Domain.Entities;

namespace CalculoLiquidaciones.Domain.Interfaces
{
    public interface IEmpleadoRepository
    {
        Task<Empleado?> ObtenerPorRutAsync(string rut);
        Task<Empleado?> ObtenerPorIdAsync(int idEmpleado);
        Task<Empleado> AgregarAsync(Empleado empleado);
    }
}
