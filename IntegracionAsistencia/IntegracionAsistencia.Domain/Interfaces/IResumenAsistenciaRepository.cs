using IntegracionAsistencia.Domain.Entities;

namespace IntegracionAsistencia.Domain.Interfaces
{
    public interface IResumenAsistenciaRepository
    {
        Task<ResumenAsistencia> GuardarResumenAsistenciaAsync(ResumenAsistencia resumen);
        Task<bool> GuardarResumenesAsistenciaMasivoAsync(IEnumerable<ResumenAsistencia> listaResumenes);
    }
}
