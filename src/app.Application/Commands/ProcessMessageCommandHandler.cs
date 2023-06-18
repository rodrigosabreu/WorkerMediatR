using app.Application.Interfaces;
using MediatR;

namespace app.Application.Commands
{
    public class ProcessMessageCommandHandler : IRequestHandler<ProcessMessageCommand>
    {
        private readonly IKafkaConsumerService _kafkaConsumerService;

        public ProcessMessageCommandHandler(IKafkaConsumerService kafkaConsumerService)
        {
            _kafkaConsumerService = kafkaConsumerService;
        }

        public async Task Handle(ProcessMessageCommand request, CancellationToken cancellationToken)
        {
            _kafkaConsumerService.StartConsumingAsync();            
        }
    }
}
