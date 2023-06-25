using app.Application.Interfaces;
using MediatR;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using app.Domain.Entities;
using app.Application.Commands;

namespace app.Application.Services
{
    public class KafkaConsumerService : IKafkaConsumerService
    {
        private readonly IMediator _mediator;
        private readonly ConsumerConfig _consumerConfig;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly ILogger<KafkaConsumerService> _logger;        

        public KafkaConsumerService(IMediator mediator, ILogger<KafkaConsumerService> logger = null)
        {
            _mediator = mediator;

            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9094",
                GroupId = "transaction-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            _cancellationTokenSource = new CancellationTokenSource();
            _logger = logger;
        }

        public async Task StartConsumingAsync()
        {
            using var consumer = new ConsumerBuilder<string, string>(_consumerConfig).Build();
            consumer.Subscribe("transacoes_financeiras");
            _logger.LogInformation("Topico subscrito em: {time}", DateTimeOffset.Now);
            bool startConsume = false;
            try
            {
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume();
                        if(!startConsume)
                            _logger.LogWarning("Consumo iniciado em: {time}", DateTimeOffset.Now);
                        startConsume = true;
                        var message = consumeResult.Message.Value;

                        //var transaction = JsonConvert.DeserializeObject<Transacao>(consumeResult.Message.Value);
                        var transaction = consumeResult.Message.Value;
                        Console.WriteLine(message);

                        await _mediator.Send(new SendMessageCommand()
                        {
                            Message = transaction
                        });

                        consumer.Commit(consumeResult);
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError("Erro em: {time}", DateTimeOffset.Now);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Erro em: {time}", DateTimeOffset.Now);
                    }
                }
            }
            finally
            {
                consumer.Close();
            }

        }

        public Task StopConsumingAsync()
        {
            _cancellationTokenSource.Cancel();
            return Task.CompletedTask;
        }
    }
}
