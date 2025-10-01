using IntegracionAsistencia.Application.Dtos;
using IntegracionAsistencia.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace IntegracionAsistencia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumenAsistenciaController : ControllerBase
    {
        private readonly IResumenAsistenciaService _resumenAsistenciaService;

        public ResumenAsistenciaController(IResumenAsistenciaService resumenAsistenciaService)
        {
            _resumenAsistenciaService = resumenAsistenciaService;
        }


        /// <summary>
        /// Genera y guarda un resumen de asistencia para un empleado (o todos) en un período.
        /// </summary>
        [HttpPost("generar")]
        public async Task<ActionResult<ResumenAsistenciaResponseDto>> GenerarAsync([FromBody] ResumenAsistenciaRequestDto requestDto)
        {
            try
            {
                var resultado = await _resumenAsistenciaService.GenerarResumenAsistenciaAsync(requestDto);
                return Ok(resultado);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al generar resúmenes", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Consulta automáticamente los resúmenes para calcular sueldos en sistema de liquidaciones
        /// </summary>
        [HttpGet("resumen-liquidacion")]
        public async Task<ActionResult<List<ResumenLiquidacionDto>>> ObtenerResumenLiquidacion([FromQuery] ResumenLiquidacionRequestDto request)
        {
            try
            {
                var resumenes = await _resumenAsistenciaService.ObtenerResumenParaLiquidacionAsync(request);

                return Ok(resumenes);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al obtener resúmenes", detalle = ex.Message });
            }
        }

        /// <summary>
        /// Obtener archivo Excel con los resúmenes para realizar carga manual en sistema de liquidaciones
        /// </summary>
        [HttpGet("resumen-liquidacion/exportar")]
        public async Task<IActionResult> ExportarResumenExcel([FromQuery] ResumenLiquidacionRequestDto request)
        {
            try
            {
                var resumenes = await _resumenAsistenciaService.ObtenerResumenParaLiquidacionAsync(request);

                using var package = new ExcelPackage();
                var worksheet = package.Workbook.Worksheets.Add("Resumen Asistencias");

                worksheet.Cells[1, 1].Value = "ID Empleado";
                worksheet.Cells[1, 2].Value = "RUT";
                worksheet.Cells[1, 3].Value = "Nombres";
                worksheet.Cells[1, 4].Value = "Apellidos";
                worksheet.Cells[1, 5].Value = "Fecha Desde";
                worksheet.Cells[1, 6].Value = "Fecha Hasta";
                worksheet.Cells[1, 7].Value = "Horas Normales";
                worksheet.Cells[1, 8].Value = "Horas Extras";
                worksheet.Cells[1, 9].Value = "Inasistencias";
                worksheet.Cells[1, 10].Value = "Licencias Médicas";
                worksheet.Cells[1, 11].Value = "Días Laborables";
                worksheet.Cells[1, 12].Value = "Días Asistidos";

                using (var range = worksheet.Cells[1, 1, 1, 12])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                int fila = 2;
                foreach (var resumen in resumenes)
                {
                    worksheet.Cells[fila, 1].Value = resumen.IdEmpleado;
                    worksheet.Cells[fila, 2].Value = resumen.RutEmpleado;
                    worksheet.Cells[fila, 3].Value = resumen.Nombres;
                    worksheet.Cells[fila, 4].Value = resumen.Apellidos;
                    worksheet.Cells[fila, 5].Value = resumen.FechaDesde.ToString("yyyy-MM-dd");
                    worksheet.Cells[fila, 6].Value = resumen.FechaHasta.ToString("yyyy-MM-dd");
                    worksheet.Cells[fila, 7].Value = resumen.TotalHorasNormales;
                    worksheet.Cells[fila, 8].Value = resumen.TotalHorasExtras;
                    worksheet.Cells[fila, 9].Value = resumen.TotalInasistencias;
                    worksheet.Cells[fila, 10].Value = resumen.TotalLicenciasMedicas;
                    worksheet.Cells[fila, 11].Value = resumen.DiasLaborables;
                    worksheet.Cells[fila, 12].Value = resumen.DiasAsistidos;
                    fila++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                var fileName = $"ResumenAsistencias_{request.IdEmpresa}_{request.FechaDesde:yyyyMMdd}_{request.FechaHasta:yyyyMMdd}.xlsx";

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al exportar", detalle = ex.Message });
            }
        }
    }
}
