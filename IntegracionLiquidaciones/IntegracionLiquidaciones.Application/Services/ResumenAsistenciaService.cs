using IntegracionLiquidaciones.Application.Dtos;
using IntegracionLiquidaciones.Application.Interfaces;
using IntegracionLiquidaciones.Domain.Interfaces;

namespace IntegracionLiquidaciones.Application.Services
{
    public class ResumenAsistenciaService : IResumenAsistenciaService
    {
        private readonly IResumenAsistenciaRepository _repo;

        public ResumenAsistenciaService(IResumenAsistenciaRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<ResumenAsistenciaDto>> ObtenerPorPeriodoAsync(string periodo, CancellationToken ct = default)
        {
            var data = await _repo.GetByPeriodoAsync(periodo, ct);
            return data.Select(x => new ResumenAsistenciaDto
            {
                RutEmpleado = x.RutEmpleado,
                Periodo = x.Periodo,
                HorasNormales = x.HorasNormales,
                HorasExtras = x.HorasExtras,
                Inasistencias = x.Inasistencias
            }).ToList();
        }
    }
}
