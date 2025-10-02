using CalculoLiquidaciones.Application.Dtos;
using CalculoLiquidaciones.Application.Interfaces;
using CalculoLiquidaciones.Domain.Entities;
using CalculoLiquidaciones.Domain.Interfaces;

namespace CalculoLiquidaciones.Application.Services
{
    public class LiquidacionService : ILiquidacionService
    {
        private readonly ILiquidacionRepository _liqRepository;
        private readonly IEmpleadoRepository _empleadoRepository;

        public LiquidacionService(ILiquidacionRepository liqRepository, IEmpleadoRepository empleadoRepository)
        {
            _liqRepository = liqRepository;
            _empleadoRepository = empleadoRepository;
        }

        public async Task<IReadOnlyList<LiquidacionResumenDto>> ObtenerResumenLiquidacionesAsync()
        {
            var entidades = await _liqRepository.ObtenerLiquidacionesAsync();

            return entidades.Select(l => new LiquidacionResumenDto
            {
                RutEmpleado = l.RutEmpleado,
                NombreCompleto = l.Empleado is null ? string.Empty: $"{l.Empleado.Nombres} {l.Empleado.Apellidos}",
                Periodo = l.Periodo,
                SueldoBase = l.SueldoBase,
                TotalHorasExtras = l.TotalHorasExtras,
                TotalDescuentos = l.TotalDescuentos,
                TotalLiquido = l.TotalLiquido
            }).ToList();
        }
    }
}
