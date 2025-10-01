using System.ComponentModel.DataAnnotations.Schema;

namespace IntegracionAsistencia.Domain.Entities
{
    public class ResumenAsistencia
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
        public int DiasLaborables { get; set; }
        public int DiasAsistidos { get; set; }
        public Guid? IdCorrelacion { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual Empleado Empleado { get; set; }
        public virtual TipoNomina TipoNomina { get; set; }
    }
}
