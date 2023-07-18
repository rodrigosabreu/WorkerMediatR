using app.Application.Commands;
using app.Application.Log;
using app.Application.Queries;
using app.Application.Util;
using MediatR;

namespace app.Worker
{
    public class Worker : BackgroundService
    {
        private ILoggerWorker<Worker> _logger;        
        private readonly IMediator _mediator;

        public Worker(ILoggerWorker<Worker> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInfo($"Iniciando o consumo do topico kafka");            

            var resp = await _mediator.Send(new GetEstruturaComercialQuery());

            if(resp)
                await _mediator.Send(new ProcessMessageCommand());
        }      

    }
}