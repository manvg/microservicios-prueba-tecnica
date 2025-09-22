using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracionAsistencia.Domain.Enums
{
    public enum TipoJornada
    {
        Normal = 1,
        ConHorasExtras = 2,
        LicenciaMedica = 3,
        AusenciaInjustificada = 4,
        Recuperacion = 5
    }
}
