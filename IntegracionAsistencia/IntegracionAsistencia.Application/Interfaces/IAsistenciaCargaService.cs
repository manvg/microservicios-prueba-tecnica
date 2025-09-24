using IntegracionAsistencia.Application.Dtos;

namespace IntegracionAsistencia.Application.Interfaces
{
    public interface IAsistenciaCargaService
    {
        Task<CargaResultadoDto> CargarAsistenciasAsync(IEnumerable<CargaAsistenciaRequestDto> asistencias);
    }
}
