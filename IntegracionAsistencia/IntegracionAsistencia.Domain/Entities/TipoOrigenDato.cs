using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracionAsistencia.Domain.Entities
{
    public class TipoOrigenDato
    {
        public int IdTipoOrigenDato { get; private set; }
        public string Nombre { get; private set; }
        public ICollection<Asistencia> Asistencias { get; private set; } = new List<Asistencia>();
    }
}
