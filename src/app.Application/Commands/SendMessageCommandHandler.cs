using app.Application.Interfaces;
using app.Domain.Entities;
using MediatR;
using Newtonsoft.Json;

namespace app.Application.Commands
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand>   
    {
        private readonly ISqsService _sqsService;

        public SendMessageCommandHandler(ISqsService sqsService)
        {
            _sqsService = sqsService;
        }

        public async Task Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var transaction = JsonConvert.DeserializeObject<Transacao>(request.Message);

            _sqsService.PublishMessage(transaction);
        }
      
    }
}
