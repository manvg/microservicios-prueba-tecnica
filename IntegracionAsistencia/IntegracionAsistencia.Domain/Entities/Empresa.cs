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
        public int IdEmpresa { get; private set; }
        public string RutEmpresa { get; private set; }
        public string RazonSocial { get; private set; }
        public int IdTipoNomina { get; private set; }
        public TipoNomina TipoNomina { get; private set; }
        public ICollection<Empleado> Empleados { get; private set; } = new List<Empleado>();

        private Empresa() { }

        public Empresa(string rutEmpresa, string razonSocial, int idTipoNomina)
        {
            RutEmpresa = rutEmpresa;
            RazonSocial = razonSocial;
            IdTipoNomina = idTipoNomina;
        }
    }
}
