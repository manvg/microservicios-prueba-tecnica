using IntegracionAsistencia.Application.Dtos;
using IntegracionAsistencia.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
    }
}
