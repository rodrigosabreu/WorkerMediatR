using app.Application.Interfaces;
using app.Domain.Entities;
using MediatR;

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
            var msg = new Product
            {
                Id = request.Id,
                Name = request.Nome,
                Quantity = request.Quantidade
            };

            _sqsService.PublishMessage(msg);
        }
      
    }
}
