using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracionLiquidaciones.Domain.Entities
{
    public class ResumenAsistencia
    {
        public int IdResumenAsistencia { get; set; }

        public string RutEmpleado { get; set; } = string.Empty;
        public string NombresEmpleado { get; set; } = string.Empty;
        public string ApellidosEmpleado { get; set; } = string.Empty;
        public string Periodo { get; set; } = string.Empty; // YYYY-MM
        public decimal SalarioBase { get; set; }
        public int HorasNormales { get; set; }
        public int HorasExtras { get; set; }
        public int Inasistencias { get; set; }
        public int DiasLicencia { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public DateTime FechaRecepcion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaEnvio { get; set; }
    }
}
