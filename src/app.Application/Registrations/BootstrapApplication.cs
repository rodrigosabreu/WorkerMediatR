using app.Application.Commands;
using app.Application.Interfaces;
using app.Application.Log;
using app.Application.Services;
using app.Domain.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace app.Application.Registrations
{
    public static class BootstrapApplication
    {
        public static IServiceCollection RegisterApplication(this IServiceCollection services)
        {
            services.AddSingleton(typeof(ILoggerWorker<>), typeof(LoggerWorker<>));
            services.AddTransient<IKafkaConsumerService, KafkaConsumerService>();
            services.AddTransient<IRequestHandler<SendMessageCommand>, SendMessageCommandHandler>();
            services.AddTransient<IRequestHandler<ProcessMessageCommand>, ProcessMessageCommandHandler>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IEstruturaComercialService, EstruturaComercialService>();
            services.AddTransient<ISqsService, SqsService>();

            return services;
        }
    }
}
