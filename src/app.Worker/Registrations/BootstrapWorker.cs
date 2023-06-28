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
                .WriteTo.EventCollector("http://localhost:8088/services/collector", "fb5fc0ee-285b-4523-bc59-d23acfcd7903")
                .CreateLogger());

            return logging;
        }
    }
}
