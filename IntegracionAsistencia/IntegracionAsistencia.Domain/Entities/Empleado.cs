using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracionAsistencia.Domain.Entities
{
    public class Empleado
    {
        public int IdEmpleado { get; private set; }
        public int IdEmpresa { get; private set; }
        public string Rut { get; private set; }
        public string Nombres { get; private set; }
        public string Apellidos { get; private set; }
        public string? Email { get; private set; }
        public string? Cargo { get; private set; }
        public string? Departamento { get; private set; }
        public DateTime FechaIngreso { get; private set; }
        public decimal SalarioBase { get; private set; }
        public TimeSpan HoraEntrada { get; private set; }
        public TimeSpan HoraSalida { get; private set; }
        public int HorasSemanales { get; private set; }
        public bool Activo { get; private set; }
        public Empresa Empresa { get; private set; }
        public ICollection<Asistencia> Asistencias { get; private set; } = new List<Asistencia>();

        private Empleado() { }

        public Empleado(int idEmpresa, string rut, string nombres, string apellidos,
            DateTime fechaIngreso, decimal salarioBase, TimeSpan horaEntrada, TimeSpan horaSalida, int horasSemanales, bool activo)
        {
            IdEmpresa = idEmpresa;
            Rut = rut;
            Nombres = nombres;
            Apellidos = apellidos;
            FechaIngreso = fechaIngreso;
            SalarioBase = salarioBase;
            HoraEntrada = horaEntrada;
            HoraSalida = horaSalida;
            HorasSemanales = horasSemanales;
            Activo = activo;
        }
    }
}
