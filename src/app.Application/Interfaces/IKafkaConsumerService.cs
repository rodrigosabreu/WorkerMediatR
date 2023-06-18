namespace app.Application.Interfaces
{
    public interface IKafkaConsumerService
    {
        Task StartConsumingAsync();
        Task StopConsumingAsync();
    }
}
