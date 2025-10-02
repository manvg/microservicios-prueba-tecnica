namespace CalculoLiquidaciones.Application.Dtos
{
    public class LiquidacionResumenDto
    {
        public string RutEmpleado { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Periodo { get; set; } = string.Empty;
        public decimal SueldoBase { get; set; }
        public decimal TotalHorasExtras { get; set; }
        public decimal TotalDescuentos { get; set; }
        public decimal TotalLiquido { get; set; }
        //public List<DetalleConceptoDto> Detalles { get; set; } = new();
    }
}
