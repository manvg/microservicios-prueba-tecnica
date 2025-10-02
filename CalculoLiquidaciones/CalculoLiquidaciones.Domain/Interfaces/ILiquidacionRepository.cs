using CalculoLiquidaciones.Domain.Entities;

namespace CalculoLiquidaciones.Domain.Interfaces
{
    public interface ILiquidacionRepository
    {
        Task<IEnumerable<Liquidacion>> ObtenerLiquidacionesAsync();
        Task GuardarLiquidacionAsync(Liquidacion liquidacion);
    }
}
