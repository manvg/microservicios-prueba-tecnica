using IntegracionAsistencia.Domain.Entities;
using IntegracionAsistencia.Domain.Interfaces;
using IntegracionAsistencia.Infrastructure.Persistence.Contexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace IntegracionAsistencia.Infrastructure.Repositories
{
    public class ResumenAsistenciaRepository : IResumenAsistenciaRepository
    {
        private readonly AppDbContext _context;

        public ResumenAsistenciaRepository(AppDbContext appDbContext) 
        {
            _context = appDbContext;
        }

        /// <summary>
        /// Inserta o actualiza un resumen de asistencia
        /// </summary>
        public async Task<ResumenAsistencia> GuardarResumenAsistenciaAsync(ResumenAsistencia resumen)
        {
            var existente = await _context.ResumenAsistencia
                .FirstOrDefaultAsync(r =>
                    r.IdEmpresa == resumen.IdEmpresa &&
                    r.IdEmpleado == resumen.IdEmpleado &&
                    r.IdTipoNomina == resumen.IdTipoNomina &&
                    r.FechaDesde == resumen.FechaDesde &&
                    r.FechaHasta == resumen.FechaHasta);

            if (existente == null)
            {
                _context.ResumenAsistencia.Add(resumen);
            }
            else
            {
                existente.HorasNormales = resumen.HorasNormales;
                existente.HorasExtras = resumen.HorasExtras;
                existente.Inasistencias = resumen.Inasistencias;
                existente.Licencias = resumen.Licencias;
                existente.IdCorrelacion = resumen.IdCorrelacion;
                existente.FechaGeneracion = DateTime.UtcNow;

                _context.ResumenAsistencia.Update(existente);
            }

            await _context.SaveChangesAsync();
            return resumen;
        }

        /// <summary>
        /// Inserta o actualiza una colección de resúmenes de asistencia en bloque.
        /// </summary>
        public async Task<bool> GuardarResumenesAsistenciaMasivoAsync(IEnumerable<ResumenAsistencia> listaResumenes)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var listaAgregar = new List<ResumenAsistencia>();
                var listaActualizar = new List<ResumenAsistencia>();
                var fechaHoy = DateTime.UtcNow;
                var listaResumen = listaResumenes.ToArray();

                if (listaResumen.Length == 0)
                    return false;

                // Obtener ids
                var empresasIds = listaResumen.Select(r => r.IdEmpresa).Distinct().ToList();
                var empleadosIds = listaResumen.Select(r => r.IdEmpleado).Distinct().ToList();
                var tiposNominaIds = listaResumen.Select(r => r.IdTipoNomina).Distinct().ToList();
                var fechasDesde = listaResumen.Select(r => r.FechaDesde).Distinct().ToList();
                var fechasHasta = listaResumen.Select(r => r.FechaHasta).Distinct().ToList();

                // Consulta a BD - Sin AsNoTracking para permitir tracking de cambios
                var existentes = await _context.ResumenAsistencia
                    .Where(r => empresasIds.Contains(r.IdEmpresa) &&
                                empleadosIds.Contains(r.IdEmpleado) &&
                                tiposNominaIds.Contains(r.IdTipoNomina) &&
                                fechasDesde.Contains(r.FechaDesde) &&
                                fechasHasta.Contains(r.FechaHasta))
                    .ToListAsync();

                // Generar diccionario con clave compuesta
                var dicExistentes = existentes.ToDictionary(
                    r => $"{r.IdEmpresa}-{r.IdEmpleado}-{r.IdTipoNomina}-{r.FechaDesde:yyyyMMdd}-{r.FechaHasta:yyyyMMdd}",
                    r => r
                );

                // Detectar duplicados dentro del mismo batch
                var clavesEnBatch = new HashSet<string>();

                foreach (var item in listaResumen)
                {
                    var clave = $"{item.IdEmpresa}-{item.IdEmpleado}-{item.IdTipoNomina}-{item.FechaDesde:yyyyMMdd}-{item.FechaHasta:yyyyMMdd}";

                    // Detectar duplicados dentro del mismo batch - omitir si ya existe
                    if (!clavesEnBatch.Add(clave))
                        continue;

                    if (dicExistentes.TryGetValue(clave, out var existente))
                    {
                        existente.HorasNormales = item.HorasNormales;
                        existente.HorasExtras = item.HorasExtras;
                        existente.Inasistencias = item.Inasistencias;
                        existente.Licencias = item.Licencias;
                        existente.IdCorrelacion = item.IdCorrelacion;
                        existente.FechaGeneracion = fechaHoy;
                        listaActualizar.Add(existente);
                    }
                    else
                    {
                        item.FechaGeneracion = fechaHoy;
                        listaAgregar.Add(item);
                    }
                }

                // Agregar nuevos registros
                if (listaAgregar.Count > 0)
                {
                    await _context.ResumenAsistencia.AddRangeAsync(listaAgregar);
                }

                // Actualizar registros
                if (listaActualizar.Count > 0)
                {
                    _context.ResumenAsistencia.UpdateRange(listaActualizar);
                }

                // Guardar
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();

                // Obtener detalle del error SQL
                var sqlException = ex.InnerException as SqlException;
                var errorDetalle = sqlException != null
                    ? $"SQL Error {sqlException.Number}: {sqlException.Message}"
                    : ex.InnerException?.Message ?? ex.Message;

                // Si tienes logger, descomenta esta línea:
                // _logger.LogError(ex, "Error al guardar resúmenes masivos: {Error}", errorDetalle);

                throw new InvalidOperationException($"Error al guardar resúmenes de asistencia: {errorDetalle}", ex);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
