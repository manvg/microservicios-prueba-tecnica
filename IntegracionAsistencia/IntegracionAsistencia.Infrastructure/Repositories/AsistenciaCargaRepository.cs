using IntegracionAsistencia.Application.Dtos;
using IntegracionAsistencia.Application.Interfaces;
using IntegracionAsistencia.Domain.Entities;
using IntegracionAsistencia.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace IntegracionAsistencia.Infrastructure.Repositories
{
    public class AsistenciaCargaRepository : IAsistenciaCargaRepository
    {
        private readonly AppDbContext _context;

        public AsistenciaCargaRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Ejecuta la carga masiva de asistencias en base de datos descartando registros duplicados por empleado y fecha.
        /// Retorna un resumen con el total recibido, insertado y duplicado.
        /// </summary>
        public async Task<CargaResultadoDto> CargarAsistenciasAsync(IEnumerable<CargaAsistenciaRequestDto> asistencias)
        {
            #region [Parámetros]
            int totalRecibidos = asistencias.Count();
            int totalInsertados = 0;
            int totalDuplicados = 0;

            var listaAsistenciaEntity = new List<Asistencia>();
            #endregion

            var asistenciasPorFecha = asistencias
                .GroupBy(a => a.Fecha.Date)
                .OrderBy(g => g.Key);

            foreach (var item in asistenciasPorFecha)
            {
                // Obtener existentes en BD solo para esa fecha
                var existentes = await _context.Asistencia
                    .Where(a => a.Fecha.Date == item.Key)
                    .Select(a => new { a.IdEmpleado, Fecha = a.Fecha.Date })
                    .ToListAsync();

                var existentesSet = new HashSet<(int, DateTime)>(
                    existentes.Select(e => (e.IdEmpleado, e.Fecha))
                );

                foreach (var dto in item)
                {
                    // Descartar asistencia existente
                    if (existentesSet.Contains((dto.IdEmpleado, dto.Fecha.Date)))
                    {
                        totalDuplicados++;
                        continue;
                    }

                    listaAsistenciaEntity.Add(new Asistencia
                    {
                        IdEmpleado = dto.IdEmpleado,
                        Fecha = dto.Fecha,
                        HoraEntrada = dto.HoraEntrada,
                        HoraSalida = dto.HoraSalida,
                        HorasTrabajadas = dto.HorasTrabajadas,
                        HorasExtras = dto.HorasExtras,
                        IdTipoJornada = dto.IdTipoJornada,
                        IdEstadoAsistencia = dto.IdEstadoAsistencia,
                        IdTipoOrigenDato = dto.IdTipoOrigenDato,
                        Observaciones = dto.Observaciones,
                        Ubicacion = dto.Ubicacion,
                        DispositivoMarcaje = dto.DispositivoMarcaje
                    });

                    totalInsertados++;
                }
            }

            // Guardar
            if (listaAsistenciaEntity.Any())
            {
                _context.Asistencia.AddRange(listaAsistenciaEntity);
                await _context.SaveChangesAsync();
            }

            return new CargaResultadoDto
            {
                TotalRecibidos = totalRecibidos,
                TotalInsertados = totalInsertados,
                TotalDuplicados = totalDuplicados,
                Mensaje = "Carga masiva procesada correctamente."
            };
        }

    }
}
