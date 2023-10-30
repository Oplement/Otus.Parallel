using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Otus.Teaching.Concurrency.Queue.Access;
using Otus.Teaching.Concurrency.Queue.Handler.QueueHandlers;
using Otus.Teaching.Concurrency.Queue.Scheduler.Schedulers;
//using Otus.Teaching.Concurrency.Queue.Scheduler.Schedulers;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine($"Scheduler started with process Id {Process.GetCurrentProcess().Id}...");
        Console.WriteLine("Generating queue...");

        GenerateQueue();
        ShowQueueInConsole(10);

        Console.WriteLine("Queue generated...");

        var scheduler = GetScheduler();

        scheduler.ProcessQueue();

        ShowQueueInConsole(10);
    }

    static IScheduler GetScheduler()
    {
        //Простой планировщик с последовательной обработкой
        return new SimpleScheduler();

        //Планировщик с параллельной обработкой на процессах
        //Console.ReadLine();
        //return new ProcessesScheduler();

        //Планировщик с параллельной обработкой на потоках
        //return new ThreadsScheduler();

        //Планировщик с параллельной обработкой на потоках
        //return new ThreadPoolScheduler();

        //Планировщик с параллельной обработкой на потоках
        //return new ThreadsBackgroundForegroundScheduler();

        //Планировщик с параллельной обработкой на tasks
        // return new TasksScheduler();
    }

    static void GenerateQueue()
    {
        using var dataContext = new DataContext();
        new DbInitializer(dataContext).Initialize();
    }

    static void ShowQueueInConsole(int consoleSize)
    {
        using var dataContext = new DataContext();
        foreach (var item in dataContext.MessageQueue.OrderBy(x => x.CreateDate).Take(consoleSize))
        {
            Console.WriteLine(item);
        }

        Console.WriteLine($"Queue count: {dataContext.MessageQueue.Count()}");
        Console.WriteLine($"Showed in console: {consoleSize}");
    }
}