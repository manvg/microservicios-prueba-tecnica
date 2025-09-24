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
        public ICollection<Empresa> Empresas { get; private set; } = new List<Empresa>();

        private TipoNomina() { }
        public TipoNomina(string nombre) => Nombre = nombre;
    }
}
