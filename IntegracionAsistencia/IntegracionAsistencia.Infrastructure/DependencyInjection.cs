using IntegracionAsistencia.Infrastructure.Messaging;
using IntegracionAsistencia.Domain.Interfaces.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IntegracionAsistencia.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration cfg)
        {
            // Bind automático de appsettings -> RabbitMqOpciones
            services.Configure<RabbitMqOpciones>(cfg.GetSection("RabbitMq"));

            // Registro del publicador como singleton
            services.AddSingleton<IEventosIntegracionPublicador, RabbitMqPublicador>();

            return services;
        }
    }
}
