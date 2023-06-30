using app.Application.Commands;
using app.Application.Interfaces;
using app.Application.Log;
using app.Domain.Entities;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace app.Application.Services
{
    public class KafkaConsumerService : IKafkaConsumerService
    {
        private readonly IMediator _mediator;
        private readonly ConsumerConfig _consumerConfig;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly ILoggerWorker<KafkaConsumerService> _logger;        

        public KafkaConsumerService(IMediator mediator, ILoggerWorker<KafkaConsumerService> logger = null)
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
            _logger.LogInfo($"Topico subscrito em: {DateTimeOffset.Now}");
            bool startConsume = false;
            try
            {
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume();
                        if(!startConsume)
                            _logger.LogInfo($"Consumo iniciado em: {DateTimeOffset.Now}");
                        startConsume = true;
                        var message = consumeResult.Message.Value;

                        var t = JsonConvert.DeserializeObject<Transacao>(consumeResult.Message.Value);
                        var transaction = consumeResult.Message.Value;

                        var dic = new Dictionary<string, object>
                        {
                            ["CustomerId"] = t.CustomerId,
                            ["TransactionId"] = t.TransactionId,
                            ["TransactionType"] = t.TransactionType,
                            ["Amount"] = t.Amount

                        };
                        
                        _logger.LogInfo(dic, "Mensagem recebida");                        

                        await _mediator.Send(new SendMessageCommand()
                        {
                            Message = transaction
                        });

                        consumer.Commit(consumeResult);
                    }                  
                    catch (Exception ex)
                    {
                        _logger.LogException("Erro ao consumir mensagem", ex);
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
