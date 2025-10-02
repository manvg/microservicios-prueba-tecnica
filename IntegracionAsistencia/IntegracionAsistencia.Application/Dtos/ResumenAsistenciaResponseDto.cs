
namespace IntegracionAsistencia.Application.Dtos
{
    public class ResumenAsistenciaResponseDto
    {
        public int TotalProcesados { get; set; } 
        public string Mensaje { get; set; } = string.Empty;
        public List<string> Errores { get; set; } = new();
        public Guid IdEvento { get; set; }
    }
}
