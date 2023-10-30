using System;
using Otus.Teaching.Concurrency.Queue.Access;
using Otus.Teaching.Concurrency.Queue.Handler.MessageHandlers;
using Otus.Teaching.Concurrency.Queue.Handler.QueueHandlers;

namespace Otus.Teaching.Concurrency.Queue.Handler
{
    public class QueueHandlerFactory
    {
        private const int HandlingDelayMilliseconds = 10;

        public static IQueueHandler GetSimpleQueueHandler()
        {
            return new SimpleQueueHandler(new FakeMessageHandler(HandlingDelayMilliseconds));
        }
        
        public static IQueueHandler GetTypeQueueHandler(MessageQueueItemType type)
        {
            return new TypeQueueHandler(new FakeMessageHandler(HandlingDelayMilliseconds), type);
        }
    }
}