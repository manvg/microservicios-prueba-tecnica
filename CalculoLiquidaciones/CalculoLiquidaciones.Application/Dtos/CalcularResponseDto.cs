using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoLiquidaciones.Application.Dtos
{
    public class CalcularResponseDto
    {
        public int TotalCalculados { get; set; }
        public List<LiquidacionResumenDto> Resultados { get; set; } = new();
    }
}
