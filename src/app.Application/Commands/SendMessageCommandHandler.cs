using app.Application.Interfaces;
using app.Application.Log;
using app.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace app.Application.Commands
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand>   
    {
        private readonly ISqsService _sqsService;
        private readonly IEstruturaComercialService _estruturaComercialService;
        private readonly ILoggerWorker<SendMessageCommandHandler> _logger;

        public SendMessageCommandHandler(ISqsService sqsService, IEstruturaComercialService estruturaComercialService, ILoggerWorker<SendMessageCommandHandler> logger)
        {
            _sqsService = sqsService;
            _estruturaComercialService = estruturaComercialService;
            _logger = logger;
        }

        public async Task Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var transaction = JsonConvert.DeserializeObject<Transacao>(request.Message);                       

            if (int.TryParse(transaction.CustomerId, out int clienteID))
            {
                var estrutura = _estruturaComercialService.GetCacheEstruturaComercial();

                _logger.LogInfo("Consultando cliente na estrutura comercial");
                if (estrutura.TryGetValue(clienteID, out _))
                {
                    _sqsService.PublishMessage(transaction, estrutura[clienteID]);
                }
                else
                {
                    var log = new Dictionary<string, object>
                    {
                        ["CustomerId"] = transaction.CustomerId,
                        ["TransactionId"] = transaction.TransactionId,
                        ["TransactionType"] = transaction.TransactionType,
                        ["Amount"] = transaction.Amount

                    };

                    _logger.LogInfo(log, "Cliente não Consta na Estrutura Comercial");                   
                }
            }
        }
      
    }
}
