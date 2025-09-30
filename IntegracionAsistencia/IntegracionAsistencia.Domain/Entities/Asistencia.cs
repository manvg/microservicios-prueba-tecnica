using IntegracionAsistencia.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracionAsistencia.Domain.Entities
{
    public class Asistencia
    {
        public int IdAsistencia { get; set; }
        public int IdEmpleado { get; set; }
        public DateTime Fecha { get; set; }
        public decimal HorasTrabajadas { get; set; }
        public decimal HorasExtras { get; set; }
        public int IdTipoJornada { get; set; }
        public int IdEstadoAsistencia { get; set; }
        public int IdTipoOrigenDato { get; set; }
        public TimeSpan? HoraEntrada { get; set; }
        public TimeSpan? HoraSalida { get; set; }
        public string? Observaciones { get; set; }
        public string? Ubicacion { get; set; }
        public string? DispositivoMarcaje { get; set; }
        public virtual Empleado Empleado { get; set; }
        public virtual TipoJornada TipoJornada { get; set; }
        public virtual EstadoAsistencia EstadoAsistencia { get; set; }
        public virtual TipoOrigenDato TipoOrigenDato { get; set; }
    }
}
