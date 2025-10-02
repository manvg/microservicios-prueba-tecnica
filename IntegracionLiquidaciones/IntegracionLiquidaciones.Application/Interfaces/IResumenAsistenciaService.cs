using IntegracionLiquidaciones.Application.Dtos;


namespace IntegracionLiquidaciones.Application.Interfaces
{
    public interface IResumenAsistenciaService
    {
        Task<List<ResumenAsistenciaDto>> ObtenerPorPeriodoAsync(string periodo, CancellationToken ct = default);
    }
}
