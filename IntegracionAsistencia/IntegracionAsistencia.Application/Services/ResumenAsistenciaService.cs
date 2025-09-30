using IntegracionAsistencia.Application.Dtos;
using IntegracionAsistencia.Application.Interfaces;
using IntegracionAsistencia.Domain.Entities;
using IntegracionAsistencia.Domain.Enums;
using IntegracionAsistencia.Domain.Interfaces;

namespace IntegracionAsistencia.Application.Services
{
    public class ResumenAsistenciaService : IResumenAsistenciaService
    {
        private readonly IAsistenciaRepository _asistenciaRepository;
        private readonly IResumenAsistenciaRepository _resumenRepository;

        public ResumenAsistenciaService(IAsistenciaRepository asistenciaRepository, IResumenAsistenciaRepository resumenAsistenciaRepository)
        {
            _asistenciaRepository = asistenciaRepository;
            _resumenRepository = resumenAsistenciaRepository;
        }

        public async Task<ResumenAsistenciaResponseDto> GenerarResumenAsistenciaAsync(ResumenAsistenciaRequestDto requestDto)
        {
            var listaResumenGenerado = new List<ResumenAsistencia>();

            var asistencias = await _asistenciaRepository.ObtenerPorPeriodoAsync(
                requestDto.IdEmpresa,
                requestDto.IdEmpleado,//null: todos los empleados
                requestDto.FechaDesde,
                requestDto.FechaHasta
            );

            if (!asistencias.Any())
                throw new InvalidOperationException("No existen asistencias para el período indicado.");

            var gruposPorEmpleado = asistencias.GroupBy(a => a.IdEmpleado);

            foreach (var grupo in gruposPorEmpleado)
            {
                decimal horasNormales = grupo
                    .Where(a => a.IdTipoJornada == (int)TipoJornadaEnum.Normal &&
                                a.HoraEntrada.HasValue &&
                                a.HoraSalida.HasValue)
                    .Sum(a => a.HorasTrabajadas);

                decimal horasExtras = grupo
                    .Where(a => a.IdTipoJornada == (int)TipoJornadaEnum.ConHorasExtras)
                    .Sum(a => a.HorasExtras);

                int licencias = grupo.Count(a => a.IdTipoJornada == (int)TipoJornadaEnum.LicenciaMedica);
                int inasistencias = grupo.Count(a => a.IdTipoJornada == (int)TipoJornadaEnum.AusenciaInjustificada);

                listaResumenGenerado.Add(new ResumenAsistencia
                {
                    IdEmpresa = requestDto.IdEmpresa,
                    IdEmpleado = grupo.Key,
                    IdTipoNomina = requestDto.IdTipoNomina,
                    FechaDesde = requestDto.FechaDesde,
                    FechaHasta = requestDto.FechaHasta,
                    HorasNormales = horasNormales,
                    HorasExtras = horasExtras,
                    Inasistencias = inasistencias,
                    Licencias = licencias,
                    IdCorrelacion = requestDto.IdCorrelacion,
                    FechaGeneracion = DateTime.UtcNow
                });

            }
            int totalProcesados = listaResumenGenerado.Count;

            await _resumenRepository.GuardarResumenesAsistenciaMasivoAsync(listaResumenGenerado);

            return new ResumenAsistenciaResponseDto
            {
                TotalProcesados = totalProcesados,
                Mensaje = $"Se procesaron {totalProcesados} resúmenes de empleados correctamente."
            };
        }
    }
}
