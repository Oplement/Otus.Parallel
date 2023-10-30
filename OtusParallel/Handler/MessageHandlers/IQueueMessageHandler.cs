using Otus.Teaching.Concurrency.Queue.Access;

namespace Otus.Teaching.Concurrency.Queue.Handler.MessageHandlers
{
    public interface IQueueMessageHandler
    {
        void Handle(MessageQueueItem item);
    }
}