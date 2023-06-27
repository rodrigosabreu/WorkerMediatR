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

                if (estrutura.TryGetValue(clienteID, out _))
                {
                    _sqsService.PublishMessage(transaction, estrutura[clienteID]);
                }
                else
                {
                    _logger.LogInfo($"Descarte: {transaction.CustomerId} -  {transaction.Amount} -  {transaction.TransactionType}");
                }
            }
        }
      
    }
}
