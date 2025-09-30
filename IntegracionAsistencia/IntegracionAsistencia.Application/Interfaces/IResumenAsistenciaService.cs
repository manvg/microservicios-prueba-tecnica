using IntegracionAsistencia.Application.Dtos;
using IntegracionAsistencia.Domain.Entities;

namespace IntegracionAsistencia.Application.Interfaces
{
    public interface IResumenAsistenciaService
    {
        Task<ResumenAsistenciaResponseDto> GenerarResumenAsistenciaAsync(ResumenAsistenciaRequestDto requestDto);
    }
}
