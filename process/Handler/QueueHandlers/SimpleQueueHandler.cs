using System;
using System.Threading;
using System.Threading.Tasks;
using Otus.Teaching.Concurrency.Queue.Access;
using Otus.Teaching.Concurrency.Queue.Handler.MessageHandlers;
using Otus.Teaching.Concurrency.Queue.Handler.QueueHandlers;

namespace Otus.Teaching.Concurrency.Queue.Handler.QueueHandlers
{
    public class SimpleQueueHandler
        : IQueueHandler
    {
        private readonly IQueueMessageHandler _messageHandler;

        public SimpleQueueHandler(IQueueMessageHandler messageHandler)
        {
            _messageHandler = messageHandler;
        }
        
        public void Handle()
        {
            using DataContext dataContext = new DataContext();
            foreach (var message in dataContext.MessageQueue)
            {
                _messageHandler.Handle(message);
                dataContext.MessageQueue.Remove(message);
            }
            
            dataContext.SaveChanges();
        }
    }
}