using app.Application.Interfaces;
using app.Domain.Entities;
using MediatR;
using Newtonsoft.Json;

namespace app.Application.Commands
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand>   
    {
        private readonly ISqsService _sqsService;
        private readonly IEstruturaComercialService _estruturaComercialService;

        public SendMessageCommandHandler(ISqsService sqsService, IEstruturaComercialService estruturaComercialService)
        {
            _sqsService = sqsService;
            _estruturaComercialService = estruturaComercialService;
        }

        public async Task Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var transaction = JsonConvert.DeserializeObject<Transacao>(request.Message);
            
            if (int.TryParse(transaction.CustomerId, out int clienteID))
            {
                var estrutura = _estruturaComercialService.GetCacheEstruturaComercial();

                if (estrutura.TryGetValue(clienteID, out _))
                {
                    _sqsService.PublishMessage(transaction);
                }
            }
        }
      
    }
}
