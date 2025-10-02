using IntegracionLiquidaciones.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IntegracionLiquidaciones.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumenAsistenciaController : ControllerBase
    {
        private readonly IResumenAsistenciaService _serviceResumenAsistencia;

        public ResumenAsistenciaController(IResumenAsistenciaService serviceResumenAsistencia)
        {
            _serviceResumenAsistencia = serviceResumenAsistencia;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string periodo, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(periodo)) return BadRequest("Debe enviar ?periodo=YYYY-MM");
            var data = await _serviceResumenAsistencia.ObtenerPorPeriodoAsync(periodo, ct);
            return Ok(data);
        }
    }
}
