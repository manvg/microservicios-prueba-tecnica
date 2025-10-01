using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoLiquidaciones.Domain.Entities
{
    public class LiquidacionDetalle
    {
        public int IdDetalle { get; set; }
        public int IdLiquidacion { get; set; }
        public string TipoConcepto { get; set; } = null!;
        public string Concepto { get; set; } = null!;
        public decimal Monto { get; set; }
        public virtual Liquidacion Liquidacion { get; set; } = null!;
    }
}
