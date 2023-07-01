using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace app.Application.LogWorker
{
    public static class LoggerStatic
    {
        private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder =>
        {
            //logging.ClearProviders();
            builder.AddSerilog(new LoggerConfiguration()
                .WriteTo.EventCollector("http://localhost:8088/services/collector", "83641cb5-7614-476e-9340-76618b02d716")
                .CreateLogger());
        });

        public static ILogger CreateLogger(string className)
        {
            return _loggerFactory.CreateLogger(className);
        }

        public static void LogInformation(string className, string message, params object[] args)
        {
            ILogger logger = CreateLogger(className);
            logger.LogInformation(message, args);
        }

        public static void LogWarning(string className, string message, params object[] args)
        {
            ILogger logger = CreateLogger(className);
            logger.LogWarning(message, args);
        }

        public static void LogError(string className, string message, params object[] args)
        {
            ILogger logger = CreateLogger(className);
            logger.LogError(message, args);
        }
    }


}
