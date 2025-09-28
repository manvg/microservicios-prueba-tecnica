namespace IntegracionAsistencia.Application.Dtos
{
    public class CargaResultadoDto
    {
        public int TotalRecibidos { get; set; }
        public int TotalInsertados { get; set; }
        public int TotalDuplicados { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public List<string> Errores { get; set; } = new List<string>();
    }
}
