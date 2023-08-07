using app.Application.Commands;
using app.Application.Interfaces;
using app.Application.Log;
using app.Application.Queries;
using MediatR;

namespace app.Worker
{
    public class Worker : BackgroundService
    {
        private ILoggerWorker<Worker> _logger;        
        private readonly IMediator _mediator;
        private readonly IKafkaConsumerTimeAtendimentoService _kafkaConsumerTimeAtendimentoService;

        public Worker(ILoggerWorker<Worker> logger, IMediator mediator, IKafkaConsumerTimeAtendimentoService kafkaConsumerTimeAtendimentoService)
        {
            _logger = logger;
            _mediator = mediator;
            _kafkaConsumerTimeAtendimentoService = kafkaConsumerTimeAtendimentoService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInfo($"Iniciando o consumo do topico kafka");

            await _kafkaConsumerTimeAtendimentoService.StartConsumingAsync();

            //var resp = await _mediator.Send(new GetEstruturaComercialQuery());

            //if(resp)
            //    await _mediator.Send(new ProcessMessageCommand());


        }      

    }
}