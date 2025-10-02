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

            // Calcular días laborables del período
            int diasLaborablesPeriodo = CalcularDiasLaborables(requestDto.FechaDesde, requestDto.FechaHasta);

            var gruposPorEmpleado = asistencias.GroupBy(a => a.IdEmpleado);

            foreach (var grupo in gruposPorEmpleado)
            {
                decimal horasNormales = grupo
                    .Where(a => (a.IdTipoJornada == (int)TipoJornadaEnum.Normal ||
                                 a.IdTipoJornada == (int)TipoJornadaEnum.ConHorasExtras) &&
                                a.HoraEntrada.HasValue &&
                                a.HoraSalida.HasValue)
                    .Sum(a => (decimal?)a.HorasTrabajadas) ?? 0;

                decimal horasExtras = grupo
                    .Where(a => a.IdTipoJornada == (int)TipoJornadaEnum.ConHorasExtras)
                    .Sum(a => (decimal?)a.HorasExtras) ?? 0;

                int licencias = grupo.Count(a => a.IdTipoJornada == (int)TipoJornadaEnum.LicenciaMedica);

                int inasistencias = grupo.Count(a => a.IdTipoJornada == (int)TipoJornadaEnum.AusenciaInjustificada);

                int diasAsistidos = grupo.Count(a => a.IdTipoJornada == (int)TipoJornadaEnum.Normal || a.IdTipoJornada == (int)TipoJornadaEnum.ConHorasExtras);

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
                    DiasLaborables = diasLaborablesPeriodo,
                    DiasAsistidos = diasAsistidos,
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

        public async Task<List<ResumenLiquidacionDto>> ObtenerResumenParaLiquidacionAsync(ResumenLiquidacionRequestDto requestDto)
        {
            var resumenes = await _resumenRepository.ObtenerResumenesPorPeriodoAsync(requestDto.IdEmpresa, requestDto.IdEmpleado, requestDto.FechaDesde, requestDto.FechaHasta, requestDto.IdTipoNomina);

            if (!resumenes.Any())
                throw new InvalidOperationException("No existen resúmenes generados para el período indicado. Debe ejecutar primero la generación de resúmenes.");

            return resumenes.Select(r => new ResumenLiquidacionDto
            {
                IdEmpleado = r.IdEmpleado,
                RutEmpleado = r.Empleado?.Rut ?? "",
                Nombres = r.Empleado?.Nombres ?? "",
                Apellidos = r.Empleado?.Apellidos ?? "",
                SalarioBase = r.Empleado?.SalarioBase ?? 0,
                FechaDesde = r.FechaDesde,
                FechaHasta = r.FechaHasta,
                TotalHorasNormales = r.HorasNormales,
                TotalHorasExtras = r.HorasExtras,
                TotalInasistencias = r.Inasistencias,
                TotalLicenciasMedicas = r.Licencias,
                DiasLaborables = r.DiasLaborables,
                DiasAsistidos = r.DiasAsistidos
            }).ToList();
        }

        //Calcula días laborables excluyendo fines de semana y feriados
        private int CalcularDiasLaborables(DateTime desde, DateTime hasta)
        {
            #region [Feriados]
            var feriados = new List<DateTime>
            {
                new DateTime(2025, 1, 1),   // Año Nuevo
                new DateTime(2025, 4, 18),  // Viernes Santo
                new DateTime(2025, 4, 19),  // Sábado Santo
                new DateTime(2025, 5, 1),   // Día del Trabajador
                new DateTime(2025, 5, 21),  // Glorias Navales
                new DateTime(2025, 6, 20),  // Día Nacional de los Pueblos Indígenas
                new DateTime(2025, 6, 29),  // San Pedro y San Pablo / Primarias
                new DateTime(2025, 7, 16),  // Virgen del Carmen
                new DateTime(2025, 8, 15),  // Asunción de la Virgen
                new DateTime(2025, 9, 18),  // Independencia Nacional
                new DateTime(2025, 9, 19),  // Glorias del Ejército
                new DateTime(2025, 10, 12), // Encuentro de Dos Mundos
                new DateTime(2025, 10, 31), // Iglesias Evangélicas y Protestantes
                new DateTime(2025, 11, 1),  // Día de Todos los Santos
                new DateTime(2025, 11, 16), // Elecciones Presidenciales / Parlamentarias
                new DateTime(2025, 12, 8),  // Inmaculada Concepción
                new DateTime(2025, 12, 14), // Segunda vuelta elecciones (por confirmar)
                new DateTime(2025, 12, 25), // Navidad
            }; 
            #endregion

            int dias = 0;
            for (var fecha = desde.Date; fecha <= hasta.Date; fecha = fecha.AddDays(1))
            {
                if (fecha.DayOfWeek != DayOfWeek.Saturday &&
                    fecha.DayOfWeek != DayOfWeek.Sunday &&
                    !feriados.Contains(fecha.Date))
                {
                    dias++;
                }
            }
            return dias;
        }
    }
}
