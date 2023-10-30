using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Otus.Teaching.Concurrency.Queue.Access;
using Otus.Teaching.Concurrency.Queue.Handler;

namespace Otus.Teaching.Concurrency.Queue.Scheduler.Schedulers
{
    public class TasksScheduler
        : IScheduler
    {
        private readonly List<TaskSchedulerItem> _queueHandlerTasks = new List<TaskSchedulerItem>();
        private readonly int _retryCount = 3;

        public void ProcessQueue()
        {
            var stopWatch = new Stopwatch();
            
            Console.WriteLine("Task scheduler...");
            Console.WriteLine("Handling queue...");
            stopWatch.Start();

            StartAsTasks();

            stopWatch.Stop();


            Console.WriteLine($"Handled queue in {stopWatch.Elapsed}...");
        }

        private void StartAsTasks()
        {
            var emailTask = new TaskSchedulerItem(MessageQueueItemType.Email);
            var smsTask = new TaskSchedulerItem(MessageQueueItemType.Sms);
            var pushTask = new TaskSchedulerItem(MessageQueueItemType.Push);
            
            _queueHandlerTasks.Add(emailTask);
            _queueHandlerTasks.Add(smsTask);
            _queueHandlerTasks.Add(pushTask);
            
            _queueHandlerTasks.ForEach(StartHandlerTask);
            
            WaitAllTasksWithRetry();
        }

        private void WaitAllTasksWithRetry()
        {
            try
            {
                var tasksForWait = GetTasksForWait();
                Task.WaitAll(tasksForWait);
            }
            catch (Exception)
            {
                RetryFaultTasks();
                WaitAllTasksWithRetry();
            }
        }

        private Task[] GetTasksForWait()
        {
            return _queueHandlerTasks
                .Where(x => x.RetryCount <= _retryCount)
                .Select(x => x.Task)
                .ToArray();
        }

        private void RetryFaultTasks()
        {
            _queueHandlerTasks
                .Where(t => t.Task.IsFaulted)
                .ToList()
                .ForEach(RetryHandle);
        }

        private void StartHandlerTask(TaskSchedulerItem item)
        {
            var handler = QueueHandlerFactory.GetTypeQueueHandler(item.Type);
            item.Task = Task.Run(() => handler.Handle());
            item.RetryCount += 1;
        }

        private void RetryHandle(TaskSchedulerItem item)
        {
            Console.WriteLine($"Пытаемся заново запустить обработчик {item.Type}, так как произошла ошибка: {item.Task?.Exception}");
            if (item.RetryCount <= _retryCount)
            {
                StartHandlerTask(item);
            }
        }
    }
}