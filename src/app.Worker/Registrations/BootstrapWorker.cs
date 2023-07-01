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

        public static ILoggingBuilder RegisterLogger(this ILoggingBuilder logging, IConfiguration config)
        {
            string url = config["SplunkLogging:Host"];
            string token = config["SplunkLogging:Token"];

            //logging.ClearProviders();
            logging.AddSerilog(new LoggerConfiguration()
                .WriteTo.EventCollector(url, token)
                .CreateLogger());

            return logging;
        }
    }
}
