using CalculoLiquidaciones.Application.Dtos;
using System.Threading.Tasks;

namespace CalculoLiquidaciones.Application.Interfaces
{
    public interface ILiquidacionService
    {
        Task<IReadOnlyList<LiquidacionResumenDto>> ObtenerResumenLiquidacionesAsync();
    }
}
