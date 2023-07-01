using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace app.Application.LogWorker
{
    public static class ApplicationLogging
    {
        public static ILoggerFactory LoggerFactory { get; set; }
        public static ILogger CreateLogger<T>() => LoggerFactory.CreateLogger<T>();
        public static ILogger CreateLogger(string name) => LoggerFactory.CreateLogger(name);        
    }

}
