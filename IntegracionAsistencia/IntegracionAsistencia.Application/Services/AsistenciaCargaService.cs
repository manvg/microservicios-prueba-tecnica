using IntegracionAsistencia.Application.Dtos;
using IntegracionAsistencia.Application.Interfaces;
using IntegracionAsistencia.Domain.Entities;
using IntegracionAsistencia.Domain.Enums;
using IntegracionAsistencia.Domain.Interfaces;
using OfficeOpenXml;
using System;
using System.Globalization;

namespace IntegracionAsistencia.Application.Services
{
    /// <summary>
    /// Servicio para la carga masiva de asistencias.
    /// </summary>
    public class AsistenciaCargaService : IAsistenciaCargaService
    {
        private readonly IAsistenciaRepository _asistenciaRepository;
        public AsistenciaCargaService(IAsistenciaRepository asistenciaRepository)
        {
            _asistenciaRepository = asistenciaRepository;
        }

        #region [Carga masiva - JSON]
        /// <summary>
        /// Ejecuta la carga masiva de asistencias. Origen de tipo JSON.
        /// </summary>
        public async Task<CargaResultadoDto> CargarAsistenciasJsonAsync(IEnumerable<CargaAsistenciaRequestDto> asistencias)
        {
            return await ProcesarCargaMasivaAsync(asistencias);
        }

        private async Task<CargaResultadoDto> ProcesarCargaMasivaAsync(IEnumerable<CargaAsistenciaRequestDto> asistencias)
        {
            #region [Parámetros]
            int totalRecibidos = asistencias.Count();
            int totalInsertados = 0;
            int totalDuplicados = 0;
            var listaAsistenciaEntity = new List<Asistencia>();
            #endregion

            // Agrupar por fecha para optimizar consultas
            var asistenciasPorFecha = asistencias
                .GroupBy(a => a.Fecha.Date)
                .OrderBy(g => g.Key);

            foreach (var item in asistenciasPorFecha)
            {
                // Obtener existentes en BD usando el repositorio (no DbContext directamente)
                var fechas = new List<DateTime> { item.Key };
                var existentes = await _asistenciaRepository.ObtenerExistentesPorFechasAsync(fechas);

                var existentesSet = new HashSet<(int, DateTime)>(existentes);

                foreach (var dto in item)
                {
                    // Descartar asistencia existente (LÓGICA DE NEGOCIO)
                    if (existentesSet.Contains((dto.IdEmpleado, dto.Fecha.Date)))
                    {
                        totalDuplicados++;
                        continue;
                    }

                    // Mapear DTO a Entidad
                    listaAsistenciaEntity.Add(new Asistencia
                    {
                        IdEmpleado = dto.IdEmpleado,
                        Fecha = dto.Fecha,
                        HoraEntrada = dto.HoraEntrada,
                        HoraSalida = dto.HoraSalida,
                        HorasTrabajadas = dto.HorasTrabajadas,
                        HorasExtras = dto.HorasExtras,
                        IdTipoJornada = dto.IdTipoJornada,
                        IdEstadoAsistencia = dto.IdEstadoAsistencia,
                        IdTipoOrigenDato = dto.IdTipoOrigenDato,
                        Observaciones = dto.Observaciones,
                        Ubicacion = dto.Ubicacion,
                        DispositivoMarcaje = dto.DispositivoMarcaje
                    });

                    totalInsertados++;
                }
            }

            // Guardar usando el repositorio
            if (listaAsistenciaEntity.Any())
            {
                await _asistenciaRepository.AgregarRangoAsync(listaAsistenciaEntity);
            }

            return new CargaResultadoDto
            {
                TotalRecibidos = totalRecibidos,
                TotalInsertados = totalInsertados,
                TotalDuplicados = totalDuplicados,
                Mensaje = "Carga masiva procesada correctamente."
            };
        }

        #endregion

        #region [Carga masiva - Excel]
        /// <summary>
        /// Ejecuta la carga masiva de asistencias. Origen de tipo Excel.
        /// </summary>
        public async Task<CargaResultadoDto> CargarAsistenciasExcelAsync(Stream stream, string nombreArchivo)
        {
            var resultado = new CargaResultadoDto();

            stream.Position = 0;
            using var package = new ExcelPackage(stream);
            var hojaExcel = package.Workbook.Worksheets[0];

            if (hojaExcel == null)
            {
                resultado.Errores.Add("No se pudo leer la hoja de Excel");
                return resultado;
            }

            var resultadoValidacion = LeerAsistenciasExcel(hojaExcel);

            if (resultadoValidacion.Errores.Any())
            {
                resultado.Errores = resultadoValidacion.Errores;
                resultado.Mensaje = "El archivo contiene errores y no se procesó.";
                return resultado;
            }

            return await ProcesarCargaMasivaAsync(resultadoValidacion.ListaAsistencias);
        }

        /// <summary>
        /// Lee y valida fila por fila del Excel. 
        /// </summary>
        private CargaAsistenciaValidacionExcelDto LeerAsistenciasExcel(ExcelWorksheet hojaExcel)
        {
            var resultado = new CargaAsistenciaValidacionExcelDto();
            var cultura = new CultureInfo("es-CL");
            var tipoJornadaEnum = Enum.GetValues(typeof(TipoJornadaEnum)).Cast<TipoJornadaEnum>().ToList();
            var estadoAsistenciaEnum = Enum.GetValues(typeof(EstadoAsistenciaEnum)).Cast<EstadoAsistenciaEnum>().ToList();

            for (int fila = 2; fila <= hojaExcel.Dimension.End.Row; fila++)
            {
                #region [Extaer datos]
                var idEmpleadoRaw = hojaExcel.Cells[fila, 1].Text?.Trim();
                var fechaRaw = hojaExcel.Cells[fila, 2].Text?.Trim();
                var horasTrabajadasRaw = hojaExcel.Cells[fila, 3].Text?.Trim();
                var horasExtrasRaw = hojaExcel.Cells[fila, 4].Text?.Trim();
                var tipoJornadaRaw = hojaExcel.Cells[fila, 5].Text?.Trim();
                var estadoRaw = hojaExcel.Cells[fila, 6].Text?.Trim();
                var origenDatoRaw = hojaExcel.Cells[fila, 7].Text?.Trim();
                var observaciones = hojaExcel.Cells[fila, 8].Text?.Trim();
                var horaEntradaRaw = hojaExcel.Cells[fila, 9].Text?.Trim();
                var horaSalidaRaw = hojaExcel.Cells[fila, 10].Text?.Trim();
                var ubicacion = hojaExcel.Cells[fila, 11].Text?.Trim();
                var dispositivo = hojaExcel.Cells[fila, 12].Text?.Trim();

                if (string.IsNullOrWhiteSpace(idEmpleadoRaw) &&
                    string.IsNullOrWhiteSpace(fechaRaw) &&
                    string.IsNullOrWhiteSpace(horasTrabajadasRaw) &&
                    string.IsNullOrWhiteSpace(horasExtrasRaw) &&
                    string.IsNullOrWhiteSpace(tipoJornadaRaw) &&
                    string.IsNullOrWhiteSpace(estadoRaw) &&
                    string.IsNullOrWhiteSpace(origenDatoRaw) &&
                    string.IsNullOrWhiteSpace(observaciones) &&
                    string.IsNullOrWhiteSpace(horaEntradaRaw) &&
                    string.IsNullOrWhiteSpace(horaSalidaRaw) &&
                    string.IsNullOrWhiteSpace(ubicacion) &&
                    string.IsNullOrWhiteSpace(dispositivo))
                {
                    continue;
                }
                #endregion

                bool filaValida = true;

                #region [Validaciones]
                // Columna 1
                if (!int.TryParse(idEmpleadoRaw, out var idEmpleado))
                {
                    resultado.Errores.Add($"Fila {fila}, Columna 1: IdEmpleado inválido.");
                    filaValida = false;
                }

                // Columna 2
                if (!DateTime.TryParse(fechaRaw, cultura, DateTimeStyles.None, out var fecha))
                {
                    resultado.Errores.Add($"Fila {fila}, Columna 2: Fecha inválida.");
                    filaValida = false;
                }
                // Columna 3
                decimal.TryParse(horasTrabajadasRaw, out var horasTrabajadas);

                // Columna 4
                decimal.TryParse(horasExtrasRaw, out var horasExtras);

                // Columna 5
                int idTipoJornada = 0;
                if (!tipoJornadaEnum.Any(tipo => tipo.ToString().ToLower() == tipoJornadaRaw!.ToLower()))
                {
                    resultado.Errores.Add($"Fila {fila}: TipoJornada '{tipoJornadaRaw}' no es válido.");
                    filaValida = false;
                }
                else
                {
                    idTipoJornada = (int)tipoJornadaEnum.Where(tipo => tipo.ToString().ToLower() == tipoJornadaRaw!.ToLower()).First();
                }

                // Columna 6
                int idEstadoAsistencia = 0;
                if (!estadoAsistenciaEnum.Any(a => a.ToString().ToLower() == estadoRaw!.ToLower()))
                {
                    resultado.Errores.Add($"Fila {fila}: TipoJornada '{estadoRaw}' no es válido.");
                    filaValida = false;
                }
                else
                {
                    idEstadoAsistencia = (int)estadoAsistenciaEnum.Where(a => a.ToString().ToLower() == estadoRaw!.ToLower()).First();
                }
                // Columna 9
                TimeSpan? horaEntrada = TimeSpan.TryParse(horaEntradaRaw, out var hEntrada) ? hEntrada : null;

                // Columna 10
                TimeSpan? horaSalida = TimeSpan.TryParse(horaSalidaRaw, out var hSalida) ? hSalida : null;
                
                if (horaEntrada != null && horaSalida != null && horaSalida < horaEntrada)
                {
                    resultado.Errores.Add($"Fila {fila}: HoraSalida no puede ser menor que HoraEntrada.");
                    filaValida = false;
                }
                #endregion

                if (filaValida)
                {
                    resultado.ListaAsistencias.Add(new CargaAsistenciaRequestDto
                    {
                        IdEmpleado = idEmpleado,
                        Fecha = fecha.Date,
                        HorasTrabajadas = horasTrabajadas,
                        HorasExtras = horasExtras,
                        IdTipoJornada = idTipoJornada,
                        IdEstadoAsistencia = idEstadoAsistencia,
                        IdTipoOrigenDato = (int)TipoOrigenDatoEnum.Excel,
                        Observaciones = observaciones,
                        HoraEntrada = horaEntrada,
                        HoraSalida = horaSalida,
                        Ubicacion = ubicacion,
                        DispositivoMarcaje = dispositivo
                    });
                }
            }

            return resultado;
        } 
        #endregion
    }
}
