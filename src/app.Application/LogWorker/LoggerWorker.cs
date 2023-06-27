using Microsoft.Extensions.Logging;

namespace app.Application.Log
{
    public class LoggerWorker<T> : ILoggerWorker<T>
    {
        private readonly ILogger<T> _logger;

        public LoggerWorker(ILogger<T> logger)
        {
            _logger = logger;
        }

        private static string GetClassName()
        {
            return typeof(T).Name;
        }

        public void LogException(string errorMessage, Exception e)
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["type"] = e.GetType().ToString(),
                ["message"] = e.Message,
                ["innerexception"] = e.InnerException,
                ["stacktrace"] = e.StackTrace

            }))_logger.LogError($"[{GetClassName()}] {errorMessage}");
        }

        public void LogInfo(string errorMessage)
        {
            _logger.LogInformation($"[{GetClassName()}] {errorMessage}");
        }

        public void LogInfo(Dictionary<string, object> logItens, string infoMessage)
        {
            using (_logger.BeginScope(logItens))_logger.LogInformation($"[{GetClassName()}] {infoMessage}");
        }

        public void LogWarning(string errorMessage)
        {
            throw new NotImplementedException();
        }
    }
}
