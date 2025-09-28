using IntegracionAsistencia.Application.Dtos;

namespace IntegracionAsistencia.Application.Interfaces
{
    public interface IAsistenciaCargaService
    {
        Task<CargaResultadoDto> CargarAsistenciasJsonAsync(IEnumerable<CargaAsistenciaRequestDto> asistencias);
        Task<CargaResultadoDto> CargarAsistenciasExcelAsync(Stream archivo, string nombreArchivo);
    }
}
