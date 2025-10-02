using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoLiquidaciones.Application.Dtos
{
    public class ImportarResponseDto
    {
        public int TotalProcesados { get; set; }
        public int TotalCreados { get; set; }
        public int TotalActualizados { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}
