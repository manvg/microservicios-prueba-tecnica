using IntegracionLiquidaciones.Domain.Entities;

namespace IntegracionLiquidaciones.Domain.Interfaces
{
    public interface IResumenAsistenciaRepository
    {
        Task UpsertAsync(ResumenAsistencia entity, CancellationToken ct = default);
        Task<List<ResumenAsistencia>> GetByPeriodoAsync(string periodo, CancellationToken ct = default);
    }
}
