using app.Application.Interfaces;
using app.Application.Log;
using app.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Transactions;

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
            var log = new Dictionary<string, object>
            {
                ["CustomerId"] = message.CustomerId,
                ["TransactionId"] = message.TransactionId,
                ["TransactionType"] = message.TransactionType,
                ["Amount"] = message.Amount

            };

            _logger.LogInfo(log, "Transacao enviada para SQS");

        }
    }
}
