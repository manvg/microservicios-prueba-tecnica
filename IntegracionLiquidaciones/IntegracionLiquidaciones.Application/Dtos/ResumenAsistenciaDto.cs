using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracionLiquidaciones.Application.Dtos
{
    public class ResumenAsistenciaDto
    {
        public string RutEmpleado { get; set; } = "";
        public string Periodo { get; set; } = "";
        public decimal HorasNormales { get; set; }
        public decimal HorasExtras { get; set; }
        public decimal Inasistencias { get; set; }
    }
}
