using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracionAsistencia.Domain.Entities
{
    public class TipoJornada
    {
        public int IdTipoJornada { get; set; }
        public string Nombre { get; set; }

        public ICollection<Asistencia> Asistencias { get; private set; } = new List<Asistencia>();
    }
}
