using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracionAsistencia.Application.Dtos
{
    public class CargaAsistenciaValidacionExcelDto
    {
        public List<CargaAsistenciaRequestDto> ListaAsistencias { get; set; } = new();
        public List<string> Errores { get; set; } = new();
    }
}
