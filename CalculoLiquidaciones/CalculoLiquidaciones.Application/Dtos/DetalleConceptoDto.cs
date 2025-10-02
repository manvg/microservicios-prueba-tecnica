using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoLiquidaciones.Application.Dtos
{
    public class DetalleConceptoDto
    {
        public string TipoConcepto { get; set; } = string.Empty;
        public string Concepto { get; set; } = string.Empty;
        public decimal Monto { get; set; }
    }
}
