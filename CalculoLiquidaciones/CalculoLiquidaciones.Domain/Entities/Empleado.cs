using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoLiquidaciones.Domain.Entities
{
    public class Empleado
    {
        public int IdEmpleado { get; set; }
        public string Rut { get; set; } = null!;
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public decimal SueldoBase { get; set; }
        public virtual ICollection<Liquidacion> Liquidacion { get; set; } = new List<Liquidacion>();
    }
}
