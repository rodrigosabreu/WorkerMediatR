using app.Application.LogWorker;
using Microsoft.Extensions.Logging;

namespace app.Application.Util
{
    public static class Util
    {
        private readonly static ILogger _logger2 = ApplicationLogging.LoggerFactory.CreateLogger(typeof(Util).FullName);
        
        private static readonly ILogger _logger = LoggerStatic.CreateLogger(typeof(Util).FullName);

        public static void Teste()
        {
            _logger2.LogInformation("=================================================> Log de Classe statica Felipe");

            _logger.LogInformation("=================================================> Log de Classe statica Rodrigo");            
        }
    }
}
