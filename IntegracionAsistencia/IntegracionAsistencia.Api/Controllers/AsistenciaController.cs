using AutoMapper;
using IntegracionAsistencia.Application.Dtos;
using IntegracionAsistencia.Application.Interfaces;
using IntegracionAsistencia.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace IntegracionAsistencia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AsistenciaController : ControllerBase
    {
        private readonly IAsistenciaService _asistenciaService;
        private readonly IMapper _mapper;
        private readonly IAsistenciaCargaService _asistenciaCargaService;
        public AsistenciaController(IAsistenciaService asistenciaService, IMapper mapper, IAsistenciaCargaService asistenciaCargaService)
        {
            _asistenciaService = asistenciaService;
            _mapper = mapper;
            _asistenciaCargaService = asistenciaCargaService;
        }

        #region [POST]
        /// <summary>
        /// Registrar asistencia individual
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<AsistenciaResponseDto>> Registrar([FromBody] AsistenciaRequestDto dto)
        {
            if (dto == null)
                return BadRequest("Debe enviar los datos de asistencia.");

            var asistencia = _mapper.Map<Asistencia>(dto);
            await _asistenciaService.RegistrarAsistenciaAsync(asistencia);

            var response = _mapper.Map<AsistenciaResponseDto>(asistencia);
            return Ok(response);
        }

        /// <summary>
        /// Registrar asistencias de forma masiva
        /// </summary>
        [HttpPost("carga-masiva")]
        public async Task<ActionResult<CargaResultadoDto>> CargarAsistencias([FromBody] IEnumerable<CargaAsistenciaRequestDto> asistencias)
        {
            if (asistencias == null || !asistencias.Any())
                return BadRequest("Debe enviar al menos un registro de asistencia.");

            var resultado = await _asistenciaCargaService.CargarAsistenciasAsync(asistencias);

            return Ok(resultado);
        }

        #endregion

        /// <summary>
        /// Obtener asistencias por empleado en un rango de fechas
        /// Ejemplo: GET api/Asistencia/empleado/5?desde=2025-09-01&hasta=2025-09-10
        /// </summary>
        [HttpGet("empleado/{idEmpleado}")]
        public async Task<ActionResult<IEnumerable<AsistenciaResponseDto>>> ObtenerPorEmpleado(
            int idEmpleado, [FromQuery] DateTime desde, [FromQuery] DateTime hasta)
        {
            var asistencias = await _asistenciaService.ObtenerPorEmpleadoAsync(idEmpleado, desde, hasta);
            var response = _mapper.Map<IEnumerable<AsistenciaResponseDto>>(asistencias);
            return Ok(response);
        }

        /// <summary>
        /// Obtener total de horas trabajadas + extras por empleado en un rango de fechas
        /// Ejemplo: GET api/Asistencia/empleado/5/total-horas?desde=2025-09-01&hasta=2025-09-10
        /// </summary>
        [HttpGet("empleado/{idEmpleado}/total-horas")]
        public async Task<ActionResult<decimal>> CalcularTotalHoras(
            int idEmpleado, [FromQuery] DateTime desde, [FromQuery] DateTime hasta)
        {
            var total = await _asistenciaService.CalcularTotalHorasAsync(idEmpleado, desde, hasta);
            return Ok(total);
        }
    }
}
