using IntegracionAsistencia.Application.Dtos;
using IntegracionAsistencia.Application.Interfaces;
using IntegracionAsistencia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracionAsistencia.Application.Services
{
    /// <summary>
    /// Servicio para la carga masiva de asistencias.
    /// </summary>
    public class AsistenciaCargaService : IAsistenciaCargaService
    {
        private readonly IAsistenciaCargaRepository _asistenciaCargaRepository;

        /// <summary>
        /// Constructor del servicio de carga de asistencias.
        /// </summary>
        public AsistenciaCargaService(IAsistenciaCargaRepository asistenciaCargaRepository)
        {
            _asistenciaCargaRepository = asistenciaCargaRepository;
        }

        /// <summary>
        /// Ejecuta la carga masiva de asistencias.
        /// </summary>
        public async Task<CargaResultadoDto> CargarAsistenciasAsync(IEnumerable<CargaAsistenciaRequestDto> asistencias)
        {
            return await _asistenciaCargaRepository.CargarAsistenciasAsync(asistencias);
        }
    }
}
