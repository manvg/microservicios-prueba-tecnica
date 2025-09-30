using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracionAsistencia.Application.Dtos
{
    public class ResumenAsistenciaRequestDto
    {
        public int IdEmpresa { get; set; }
        public int? IdEmpleado { get; set; }
        public int IdTipoNomina { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string? IdCorrelacion { get; set; }
    }
}
