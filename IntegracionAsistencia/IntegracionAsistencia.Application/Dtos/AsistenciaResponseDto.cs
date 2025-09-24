namespace IntegracionAsistencia.Application.Dtos
{
    public class AsistenciaResponseDto
    {
        public int IdAsistencia { get; set; }
        public int IdEmpleado { get; set; }
        public DateTime Fecha { get; set; }
        public decimal HorasTrabajadas { get; set; }
        public decimal HorasExtras { get; set; }
        public int IdTipoJornada { get; set; }
        public int IdEstadoAsistencia { get; set; }
        public int IdTipoOrigenDato { get; set; }
        public string? Observaciones { get; set; }
        public string? Ubicacion { get; set; }
        public string? DispositivoMarcaje { get; set; }

        // Datos adicionales que pueden ser útiles para el cliente
        public string? NombreTipoJornada { get; set; }
        public string? NombreEstadoAsistencia { get; set; }
        public string? NombreTipoOrigenDato { get; set; }
    }
}
