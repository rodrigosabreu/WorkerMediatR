using MediatR;

namespace app.Application.Commands
{
    public class SendMessageCommand : IRequest
    {
        public string? Message { get; set; }
        
    }
}
