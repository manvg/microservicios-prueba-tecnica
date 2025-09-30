using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracionAsistencia.Domain.Entities
{
    public class TipoNomina
    {
        public int IdTipoNomina { get; private set; }
        public string Nombre { get; private set; }
        public virtual ICollection<Empresa> Empresas { get; set; } = new List<Empresa>();
        public virtual ICollection<ResumenAsistencia> ResumenesAsistencia { get; set; } = new List<ResumenAsistencia>();
    }
}
