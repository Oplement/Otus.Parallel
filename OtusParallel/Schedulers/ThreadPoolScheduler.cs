using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Otus.Teaching.Concurrency.Queue.Access;
using Otus.Teaching.Concurrency.Queue.Handler;

namespace Otus.Teaching.Concurrency.Queue.Scheduler.Schedulers
{
    class ThreadPoolScheduler
        : IScheduler
    {
        private readonly Dictionary<MessageQueueItemType,ThreadPoolSchedulerItem> _threadPoolSchedulerItems = 
            new Dictionary<MessageQueueItemType, ThreadPoolSchedulerItem> 
        {
            {MessageQueueItemType.Email, new ThreadPoolSchedulerItem(MessageQueueItemType.Email, OnThreadFinished)},
            {MessageQueueItemType.Sms, new ThreadPoolSchedulerItem(MessageQueueItemType.Sms, OnThreadFinished)},
            {MessageQueueItemType.Push, new ThreadPoolSchedulerItem(MessageQueueItemType.Push, OnThreadFinished)},
        };

        private static void OnThreadFinished(MessageQueueItemType type)
        {
            Console.WriteLine($"{type} FINISHED");
        }

        public void ProcessQueue()
        {
            var stopWatch = new Stopwatch();

            Console.WriteLine($"Thread pool scheduler in thread: {Thread.CurrentThread.ManagedThreadId}...");
            Console.WriteLine("Handling queue...");
            stopWatch.Start();

            ThreadPool.QueueUserWorkItem(HandleInThreadPool, _threadPoolSchedulerItems[MessageQueueItemType.Email]);
            ThreadPool.QueueUserWorkItem(HandleInThreadPool, _threadPoolSchedulerItems[MessageQueueItemType.Sms]);
            ThreadPool.QueueUserWorkItem(HandleInThreadPool, _threadPoolSchedulerItems[MessageQueueItemType.Push]);

            WaitHandle[] waitHandles = _threadPoolSchedulerItems.Values.Select(x => x.WaitHandle).ToArray();
            
            WaitHandle.WaitAll(waitHandles);
            
            stopWatch.Stop();

            Console.WriteLine($"Handled queue in {stopWatch.Elapsed}...");
        }
        
        private void HandleInThreadPool(object item)
        {
            var schedulerItem = (ThreadPoolSchedulerItem) item;
            var handler = QueueHandlerFactory.GetTypeQueueHandler(schedulerItem.Type);
            
            handler.Handle();

            var autoResetEvent = (AutoResetEvent) schedulerItem.WaitHandle;
            schedulerItem.OnFinish();
            
            autoResetEvent.Set();
        }
    }
}
