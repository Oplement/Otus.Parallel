using Otus.Teaching.Concurrency.Queue.Access;
using System;
using System.Threading;

namespace Otus.Teaching.Concurrency.Queue.Scheduler.Schedulers
{
    public class ThreadPoolSchedulerItem
    {
        public MessageQueueItemType Type { get; private set; }

        public WaitHandle WaitHandle { get; private set; }

        public Action<MessageQueueItemType> OnFinished { get; set; }

        public ThreadPoolSchedulerItem(MessageQueueItemType type, Action<MessageQueueItemType> onFinishCallback)
        {
            Type = type;
            WaitHandle = new AutoResetEvent(false);
            OnFinished = onFinishCallback;
        }

        public void OnFinish()
        {
            OnFinished(Type);
        }
    }
}