using app.Application.Commands;
using app.Application.Interfaces;
using app.Application.Log;
using app.Domain.Entities;
using Confluent.Kafka;
using MediatR;
using Newtonsoft.Json;

namespace app.Application.Services
{
    public class KafkaConsumerTimeAtendimentoPrivateService : IKafkaConsumerTimeAtendimentoService
    {
        private readonly IMediator _mediator;
        private readonly ConsumerConfig _consumerConfig;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly ILoggerWorker<KafkaConsumerTimeAtendimentoPrivateService> _logger;
        private readonly ITimeAtendimentoPrivateRepository _repositoryTimeAtendimentoPrivate;

        public KafkaConsumerTimeAtendimentoPrivateService(IMediator mediator, ILoggerWorker<KafkaConsumerTimeAtendimentoPrivateService> logger, ITimeAtendimentoPrivateRepository repositoryTimeAtendimentoPrivate)
        {
            _mediator = mediator;

            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9094",
                GroupId = "movimentacoes-time-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            _cancellationTokenSource = new CancellationTokenSource();
            _logger = logger;
            _repositoryTimeAtendimentoPrivate = repositoryTimeAtendimentoPrivate;
        }

        public async Task StartConsumingAsync()
        {
            using var consumer = new ConsumerBuilder<string, string>(_consumerConfig).Build();
            consumer.Subscribe("movimentacoes_time");
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

                        var t = JsonConvert.DeserializeObject<TimeAtendimentoPrivate>(consumeResult.Message.Value);
                        var transaction = consumeResult.Message.Value;

                        var dic = new Dictionary<string, object>
                        {
                            ["Cliente"] = t.Cliente,
                            ["Banker"] = t.TimeAtendimento.Banker,
                            ["Gerente"] = t.TimeAtendimento.Gerente,
                            ["Especialista"] = t.TimeAtendimento.Especialista,

                        };
                        
                        _logger.LogInfo(dic, "Mensagem recebida");
                        var resp = _repositoryTimeAtendimentoPrivate.InserirTimeAtendimentoPrivate(t);
                        //await _mediator.Send(new SendMessageCommand()
                        //{
                        //    Message = transaction
                        //});

                        //consumer.Commit(consumeResult);
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
