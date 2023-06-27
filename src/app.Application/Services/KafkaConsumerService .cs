﻿using app.Application.Commands;
using app.Application.Interfaces;
using app.Application.Log;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Logging;

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

                        //var t = JsonConvert.DeserializeObject<Transacao>(consumeResult.Message.Value);
                        var transaction = consumeResult.Message.Value;                       

                        _logger.LogInfo($"Mensagem: {transaction}");

                        await _mediator.Send(new SendMessageCommand()
                        {
                            Message = transaction
                        });

                        //consumer.Commit(consumeResult);
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogException("Erro em: {DateTimeOffset.Now}", ex);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogException("Erro em: {DateTimeOffset.Now}", ex);
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
