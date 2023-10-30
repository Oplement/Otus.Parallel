using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Otus.Teaching.Concurrency.Queue.Access;

namespace Otus.Teaching.Concurrency.Queue.Handler.Process
{
    class Program
    {
        private static MessageQueueItemType _messageType;
        private static bool _isGenerateDb;

        static void Main(string[] args)
        {
            HandleArgs(args);

            var stopWatch = new Stopwatch();

            stopWatch.Start();

            Console.WriteLine($"Handling queue for type: {_messageType}...");

            var queueHandler = QueueHandlerFactory.GetTypeQueueHandler(_messageType);

            queueHandler.Handle();

            stopWatch.Stop();

            Console.WriteLine($"Handled queue for type {_messageType} in {stopWatch.Elapsed}...");
        }


        private static void HandleArgs(string[] args)
        {
            if (args == null || !args.Any())
                throw new ArgumentException("Required command line arguments");

            string typeStr = "";
            string generateDbStr = "";
            if (args.Length == 1)
            {
                typeStr = args[0];
            }
            else if (args.Length == 2)
            {
                typeStr = args[0];
                generateDbStr = args[1];
            }
            else
            {
                throw new ArgumentException("Required right command line arguments");
            }

            if (!MessageQueueItemType.TryParse(typeStr, true, out _messageType))
                throw new ArgumentException("Not valid message type");

            Console.WriteLine($"Started queue handler for type: {_messageType} with process Id {System.Diagnostics.Process.GetCurrentProcess().Id}!");

            if (!string.IsNullOrWhiteSpace(generateDbStr))
            {
                if (bool.TryParse(generateDbStr, out _isGenerateDb))
                {
                    if (_isGenerateDb)
                    {
                        using var dataContext = new DataContext();
                        new DbInitializer(dataContext).Initialize();
                    }
                }
            }
        }
    }
}