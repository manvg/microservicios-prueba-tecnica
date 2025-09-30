namespace IntegracionAsistencia.Application.Dtos
{
    public class CargaAsistenciaRequestDto
    {
        public int IdEmpleado { get; set; }
        public DateTime Fecha { get; set; }
        public decimal HorasTrabajadas { get; set; }
        public decimal HorasExtras { get; set; }
        public int IdTipoJornada { get; set; }
        public int IdEstadoAsistencia { get; set; }
        public int IdTipoOrigenDato { get; set; }
        public string? Observaciones { get; set; }
        public TimeSpan? HoraEntrada { get; set; }
        public TimeSpan? HoraSalida { get; set; }
        public string? Ubicacion { get; set; }
        public string? DispositivoMarcaje { get; set; }
    }
}
