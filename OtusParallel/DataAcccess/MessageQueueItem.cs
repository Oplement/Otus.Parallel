using System;

namespace Otus.Teaching.Concurrency.Queue.Access
{
    public class MessageQueueItem
    {
        public Guid Id { get; set; }

        public DateTime CreateDate { get; set; }

        public Guid UserId { get; set; }

        public MessageQueueItemType Type { get; set; }

        public override string ToString()
        {
            return $"Message. Id: {Id}, Type: {Type}, Created date: {CreateDate}, User Id: {UserId}";
        }
    }
}