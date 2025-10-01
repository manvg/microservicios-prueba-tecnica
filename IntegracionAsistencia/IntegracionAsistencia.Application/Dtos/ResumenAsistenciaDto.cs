using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracionAsistencia.Application.Dtos
{
    public class ResumenAsistenciaDto
    {
        public int IdResumenAsistencia { get; set; }
        public int IdEmpresa { get; set; }
        public int IdEmpleado { get; set; }
        public int IdTipoNomina { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public decimal HorasNormales { get; set; }
        public decimal HorasExtras { get; set; }
        public int Inasistencias { get; set; }
        public int Licencias { get; set; }
        public Guid? IdCorrelacion { get; set; }
        public DateTime FechaGeneracion { get; set; }
    }
}
