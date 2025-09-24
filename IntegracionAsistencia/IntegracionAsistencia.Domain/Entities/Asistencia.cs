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

        public Empleado Empleado { get; set; }
        public TipoJornada TipoJornada { get; set; }
        public EstadoAsistencia EstadoAsistencia { get; set; }
        public TipoOrigenDato TipoOrigenDato { get; set; }
    }
}
