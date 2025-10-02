
namespace IntegracionAsistencia.Domain.Events
{
    public class EventoTotalesAsistenciaConsolidados
    {
        public Guid IdEvento { get; set; }
        public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
        public string RutEmpleado { get; set; }
        public string NombresEmpleado { get; set; }
        public string ApellidosEmpleado { get; set; }
        public string Periodo { get; set; }// YYYY-MM
        public decimal SalarioBase { get; set; }
        public int HorasNormales { get; set; }
        public int HorasExtras { get; set; }
        public int Inasistencias { get; set; }
        public int DiasLicencia { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public DateTime FechaRecepcion { get; set; } = DateTime.UtcNow;
        public DateTime? FechaEnvio { get; set; }
    }
}
