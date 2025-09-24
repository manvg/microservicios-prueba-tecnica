using IntegracionAsistencia.Application.Dtos;

namespace IntegracionAsistencia.Application.Interfaces
{
    public interface IAsistenciaCargaRepository
    {
        Task<CargaResultadoDto> CargarAsistenciasAsync(IEnumerable<CargaAsistenciaRequestDto> asistencias);
    }
}
