using Serilog;
using System.Reflection;
namespace app.Worker.Registrations
{
    public static class BootstrapWorker
    {
        public static IServiceCollection RegisterWorker(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddMemoryCache();

            return services;
        }

        public static ILoggingBuilder RegisterLogger(this ILoggingBuilder logging)
        {
            //logging.ClearProviders();
            logging.AddSerilog(new LoggerConfiguration()
                .WriteTo.EventCollector("http://localhost:8088/services/collector", "904f9c60-1815-4a64-b63f-ba9fef00204b")
                .CreateLogger());

            return logging;
        }
    }
}
