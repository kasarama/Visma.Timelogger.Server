using MediatR;

namespace Visma.Timelogger.Application.Features
{
    public abstract class Message<T> : IRequest<T>
    {
        public Guid RequestId { get; } = Guid.NewGuid();
        public Guid UserId { get; set; }
    }
}
