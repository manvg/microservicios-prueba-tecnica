using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracionAsistencia.Application.Dtos
{
    public class ResumenLiquidacionDto
    {
        public int IdEmpleado { get; set; }
        public string RutEmpleado { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public decimal SalarioBase { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public decimal TotalHorasNormales { get; set; }
        public decimal TotalHorasExtras { get; set; }
        public int TotalInasistencias { get; set; }
        public int TotalLicenciasMedicas { get; set; }
        public int DiasLaborables { get; set; }
        public int DiasAsistidos { get; set; }
    }
}
