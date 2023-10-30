using System;
using System.Diagnostics;
using Otus.Teaching.Concurrency.Queue.Handler;

namespace Otus.Teaching.Concurrency.Queue.Scheduler.Schedulers
{
    public class SimpleScheduler
        : IScheduler
    {
        
        public void ProcessQueue()
        {
            var queueHandler = QueueHandlerFactory.GetSimpleQueueHandler();
            
            var stopWatch = new Stopwatch();
            
            Console.WriteLine("Simple scheduler...");
            Console.WriteLine("Handling queue...");
            stopWatch.Start();
            
            queueHandler.Handle();
            
            stopWatch.Stop();
            
            Console.WriteLine($"Handled queue in {stopWatch.Elapsed}...");
        }
    }
}