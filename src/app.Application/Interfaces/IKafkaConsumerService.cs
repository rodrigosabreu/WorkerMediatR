﻿namespace app.Application.Interfaces
{
    public interface IKafkaConsumerTimeAtendimentoService
    {
        Task StartConsumingAsync();
        Task StopConsumingAsync();
    }
}
