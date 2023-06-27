using app.Application.Interfaces;
using app.Application.Log;
using app.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace app.Domain.Services
{
    public class SqsService : ISqsService
    {
        private readonly ILoggerWorker<SqsService> _logger;

        public SqsService(ILoggerWorker<SqsService> logger)
        {
            _logger = logger;
        }

        public void PublishMessage(Transacao message, EstruturaComercial estrutura)
        {
            // Lógica para publicar mensagem no Amazon SQS
            _logger.LogInfo($"SQS: {message.CustomerId}, {message.Amount}, {message.TransactionType}, {estrutura.NomeEspecialista}, {estrutura.EmailEspecialista}");
            
        }
    }
}
