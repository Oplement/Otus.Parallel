using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Otus.Teaching.Concurrency.Queue.Access;
using Otus.Teaching.Concurrency.Queue.Handler.MessageHandlers;

namespace Otus.Teaching.Concurrency.Queue.Handler.QueueHandlers
{
    public class TypeQueueHandler
        : IQueueHandler
    {
        private readonly IQueueMessageHandler _messageHandler;
        private readonly MessageQueueItemType _type;

        public TypeQueueHandler(IQueueMessageHandler messageHandler, MessageQueueItemType type)
        {
            _messageHandler = messageHandler;
            _type = type;
        }

        public void Handle()
        {
            Console.WriteLine($"Handler started for type: {_type} in process: {Process.GetCurrentProcess().Id} " +
                              $"in domain {AppDomain.CurrentDomain.Id} in thread: {Thread.CurrentThread.ManagedThreadId}, " +
                              $"thread from pool: {Thread.CurrentThread.IsThreadPoolThread}");

            using DataContext dataContext = new DataContext();
            var messages = dataContext.MessageQueue.Where(x => x.Type == _type).ToList();
            foreach (var message in messages)
            {
                _messageHandler.Handle(message);
            }
            
            dataContext.MessageQueue.RemoveRange(messages);
            dataContext.SaveChanges();
            
            Console.WriteLine($"Handled messages: {messages.Count}");
        }
    }
}