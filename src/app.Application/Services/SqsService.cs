using app.Application.Interfaces;
using app.Domain.Entities;

namespace app.Domain.Services
{
    public class SqsService : ISqsService
    {
        public void PublishMessage(Product message)
        {
            // Lógica para publicar mensagem no Amazon SQS
            Console.WriteLine($"Published message: {message.Id}, {message.Name}, {message.Name}");
        }
    }
}
