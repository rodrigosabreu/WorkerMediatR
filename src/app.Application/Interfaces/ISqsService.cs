using app.Domain.Entities;

namespace app.Application.Interfaces
{
    public interface ISqsService
    {
        void PublishMessage(Transacao transacao);
    }
}
