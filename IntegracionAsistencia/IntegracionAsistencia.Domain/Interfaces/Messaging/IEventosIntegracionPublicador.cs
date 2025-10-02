namespace IntegracionAsistencia.Domain.Interfaces.Messaging
{
    public interface IEventosIntegracionPublicador
    {
        Task PublicarAsync<T>(T evento, CancellationToken ct = default);
    }
}
