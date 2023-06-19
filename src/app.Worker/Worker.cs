using app.Application.Commands;
using app.Application.Queries;
using MediatR;

namespace app.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMediator _mediator;        

        public Worker(ILogger<Worker> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            await _mediator.Send(new GetEstruturaComercialQuery());

            await _mediator.Send(new ProcessMessageCommand());
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