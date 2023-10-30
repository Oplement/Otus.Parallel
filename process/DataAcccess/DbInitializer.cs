using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Otus.Teaching.Concurrency.Queue.Access
{
    public class DbInitializer
    {
        private readonly Random _random;
        private readonly DataContext _dataContext;
        private const int QueueItemsCount = 1000;

        public DbInitializer(DataContext dataContext)
        {
            _dataContext = dataContext;
            _random = new Random();
        }

        public void Initialize()
        {
            _dataContext.Database.EnsureDeleted();
            _dataContext.Database.EnsureCreated();

            var queue = new List<MessageQueueItem>();
            var now = DateTime.Now;
            for (int i = 1; i <= QueueItemsCount; i++)
            {
                var message = new MessageQueueItem()
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    CreateDate = now.AddMinutes(i),
                    Type = GetRandomMessageType()
                };
                
                queue.Add(message);
            }

            _dataContext.MessageQueue.AddRange(queue);
            _dataContext.SaveChanges();
        }

        private MessageQueueItemType GetRandomMessageType()
        {
            var typeValue = _random.Next(1, 4);
            return (MessageQueueItemType) typeValue;
        }
    }
}