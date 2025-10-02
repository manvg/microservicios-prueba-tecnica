using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoLiquidaciones.Application.Dtos
{
    public class CalcularRequestDto
    {
        public string Periodo { get; set; } = string.Empty;
        public List<string>? Ruts { get; set; }
    }
}
