using AutoMapper;
using IntegracionAsistencia.Api.Dtos;
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
        public AsistenciaController(IAsistenciaService asistenciaService, IMapper mapper)
        {
            _asistenciaService = asistenciaService;
            _mapper = mapper;
        }

        // POST: api/Asistencia
        [HttpPost]
        public async Task<ActionResult<AsistenciaResponseDto>> Registrar([FromBody] AsistenciaRequestDto dto)
        {
            var asistencia = _mapper.Map<Asistencia>(dto);
            await _asistenciaService.RegistrarAsistenciaAsync(asistencia);

            var response = _mapper.Map<AsistenciaResponseDto>(asistencia);
            return Ok(response);
        }

        // GET: api/Asistencia/empleado/5?desde=2025-09-01&hasta=2025-09-10
        [HttpGet("empleado/{idEmpleado}")]
        public async Task<ActionResult<IEnumerable<AsistenciaResponseDto>>> ObtenerPorEmpleado(
            int idEmpleado, [FromQuery] DateTime desde, [FromQuery] DateTime hasta)
        {
            var asistencias = await _asistenciaService.ObtenerPorEmpleadoAsync(idEmpleado, desde, hasta);
            var response = _mapper.Map<IEnumerable<AsistenciaResponseDto>>(asistencias);
            return Ok(response);
        }

        // GET: api/Asistencia/empleado/5/total?desde=2025-09-01&hasta=2025-09-10
        [HttpGet("empleado/{idEmpleado}/total-horas")]
        public async Task<ActionResult<decimal>> CalcularTotalHoras(
            int idEmpleado, [FromQuery] DateTime desde, [FromQuery] DateTime hasta)
        {
            var total = await _asistenciaService.CalcularTotalHorasAsync(idEmpleado, desde, hasta);
            return Ok(total);
        }
    }
}
