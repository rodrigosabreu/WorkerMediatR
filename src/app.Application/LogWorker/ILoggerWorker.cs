namespace app.Application.Log
{
    public interface ILoggerWorker<out T>
    {
        void LogException(string errorMessage, Exception e);
        void LogWarning(string errorMessage);
        void LogInfo(string errorMessage);
        void LogInfo(Dictionary<string, object> logItens, string infoMessage);
    }
}
