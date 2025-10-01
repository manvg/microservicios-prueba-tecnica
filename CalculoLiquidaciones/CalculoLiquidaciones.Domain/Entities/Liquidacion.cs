namespace CalculoLiquidaciones.Domain.Entities
{
    public class Liquidacion
    {
        public int IdLiquidacion { get; set; }
        public int IdEmpleado { get; set; }
        public string RutEmpleado { get; set; } = null!;
        public string Periodo { get; set; } = null!;
        public decimal SueldoBase { get; set; }
        public decimal TotalHorasExtras { get; set; }
        public decimal TotalDescuentos { get; set; }
        public decimal TotalLiquido { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public Guid? IdCorrelacion { get; set; }
        public virtual Empleado Empleado { get; set; } = null!;
        public virtual ICollection<LiquidacionDetalle> LiquidacionDetalle { get; set; } = new List<LiquidacionDetalle>();
    }
}
