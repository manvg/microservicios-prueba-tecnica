using IntegracionAsistencia.Domain.Events;
using IntegracionAsistencia.Domain.Interfaces.Messaging;
using IntegracionAsistencia.Infrastructure.Messaging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace IntegracionAsistencia.Infrastructure.Messaging;

public class RabbitMqPublicador : IEventosIntegracionPublicador
{
    private readonly RabbitMqOpciones _opciones;
    private readonly ConnectionFactory _factory;

    public RabbitMqPublicador(IOptions<RabbitMqOpciones> opciones)
    {
        _opciones = opciones.Value;

        _factory = new ConnectionFactory
        {
            HostName = _opciones.HostName,
            Port = _opciones.Port,
            UserName = _opciones.UserName,
            Password = _opciones.Password
        };

        if (_opciones.UseSsl)
        {
            _factory.Ssl = new SslOption
            {
                Enabled = true,
                ServerName = _opciones.HostName
            };
        }
    }

    public async Task PublicarAsync<T>(T evento, CancellationToken ct = default)
    {
        await using var connection = await _factory.CreateConnectionAsync("publisher-connection");
        await using var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(_opciones.Exchange, ExchangeType.Direct, durable: true);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(evento));

        var props = new BasicProperties
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent
        };

        await channel.BasicPublishAsync(
            exchange: _opciones.Exchange,
            routingKey: _opciones.RoutingKey,
            mandatory: false,
            basicProperties: props,
            body: body
        );
    }
}
