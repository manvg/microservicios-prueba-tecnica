using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracionAsistencia.Domain.Entities
{
    public class Empleado
    {
        public int IdEmpleado { get; set; }
        public int IdEmpresa { get; set; }
        public string Rut { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string? Email { get; set; }
        public string? Cargo { get; set; }
        public string? Departamento { get; set; }
        public DateTime FechaIngreso { get; set; }
        public decimal SalarioBase { get; set; }
        public TimeSpan HoraEntrada { get; set; }
        public TimeSpan HoraSalida { get; set; }
        public int HorasSemanales { get; set; }
        public bool Activo { get; set; }

        public virtual Empresa Empresa { get; set; }
        public virtual ICollection<Asistencia> Asistencias { get; set; } = new List<Asistencia>();
        public virtual ICollection<ResumenAsistencia> ResumenesAsistencia { get; set; } = new List<ResumenAsistencia>();
    }
}
