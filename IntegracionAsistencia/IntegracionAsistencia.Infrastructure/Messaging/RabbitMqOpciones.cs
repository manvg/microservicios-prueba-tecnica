namespace IntegracionAsistencia.Infrastructure.Messaging
{
    public class RabbitMqOpciones
    {
        public string HostName { get; set; } = "localhost";
        public int Port { get; set; } = 5672;
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string Exchange { get; set; } = "asistencia.exchange";
        public string RoutingKey { get; set; } = "asistencia.consolidada";
        public bool HabilitarConfirmaciones { get; set; } = true;
        public bool UseSsl { get; set; } = false;
    }
}
