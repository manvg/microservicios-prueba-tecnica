using IntegracionAsistencia.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracionAsistencia.Domain.Entities
{
    public class Empresa
    {
        public int IdEmpresa { get; set; }
        public string RutEmpresa { get; set; }
        public string RazonSocial { get; set; }
        public int IdTipoNomina { get; set; }

        public virtual TipoNomina TipoNomina { get; set; }
        public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
        public virtual ICollection<ResumenAsistencia> ResumenesAsistencia { get; set; } = new List<ResumenAsistencia>();
    }
}
