using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracionAsistencia.Application.Dtos
{
    public class AsistenciaExcelRow
    {
        public int IdEmpleado { get; set; }
        public DateTime Fecha { get; set; }
        public decimal HorasTrabajadas { get; set; }
        public decimal HorasExtras { get; set; }
        public int IdTipoJornada { get; set; }
        public int IdEstadoAsistencia { get; set; }
        public int IdTipoOrigenDato { get; set; }
        public string? Observaciones { get; set; }
        public DateTime? HoraEntrada { get; set; }
        public DateTime? HoraSalida { get; set; }
        public string? Ubicacion { get; set; }
        public string? DispositivoMarcaje { get; set; }
    }
}
