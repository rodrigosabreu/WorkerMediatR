using app.Application.Queries;
using app.Infrastructure.Log;
using MediatR;

namespace app.Worker
{
    public class Worker : BackgroundService
    {
        private ILogger<Worker> _logger;
        private ILogManager _loggerManager;

        private readonly IMediator _mediator;

        public Worker(ILogger<Worker> logger, IMediator mediator, ILogManager loggerManager)
        {
            _logger = logger;
            _mediator = mediator;
            _loggerManager = loggerManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {         
            
            _loggerManager.LogInformation($"{typeof(Worker)}Worker running at: {DateTimeOffset.Now}");

            await _mediator.Send(new GetEstruturaComercialQuery());

            //await _mediator.Send(new ProcessMessageCommand());
        }













        //consulta a base uma unica vez, e coloca os dados em cache para ser acessado em outras partes da aplicação
        /*await _mediator.Send(new GetEstruturaComercialQuery());

        while (!stoppingToken.IsCancellationRequested)
        {
            var products = await _mediator.Send(new GetProductsQuery());

            var messages = products.Select(product => new SendMessageCommand
            {
                Id = product.Id,
                Nome = product.Name,
                Quantidade = product.Quantity
            });

            await Task.WhenAll(messages.Select(msg => _mediator.Send(msg)));                

            await Task.Delay(1000, stoppingToken);

        }*/

    }
}