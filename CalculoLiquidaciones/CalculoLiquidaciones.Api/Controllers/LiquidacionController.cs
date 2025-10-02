using CalculoLiquidaciones.Application.Dtos;
using CalculoLiquidaciones.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CalculoLiquidaciones.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LiquidacionController : ControllerBase
    {
        private readonly ILiquidacionService _service;

        public LiquidacionController(ILiquidacionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _service.ObtenerResumenLiquidacionesAsync();
            return Ok(data);
        }
    }
}