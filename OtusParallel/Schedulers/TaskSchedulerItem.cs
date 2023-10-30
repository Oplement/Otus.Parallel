using System.Threading.Tasks;
using Otus.Teaching.Concurrency.Queue.Access;

namespace Otus.Teaching.Concurrency.Queue.Scheduler.Schedulers
{
    public class TaskSchedulerItem
    {
        public MessageQueueItemType Type { get; set; }

        public Task Task { get; set; }

        public int RetryCount { get; set; }

        public TaskSchedulerItem(MessageQueueItemType type)
        {
            Type = type;
        }
    }
}