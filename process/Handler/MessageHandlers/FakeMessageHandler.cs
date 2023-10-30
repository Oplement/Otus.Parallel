using System.Threading;
using Otus.Teaching.Concurrency.Queue.Access;

namespace Otus.Teaching.Concurrency.Queue.Handler.MessageHandlers
{
    public class FakeMessageHandler
        : IQueueMessageHandler
    {
        private readonly int _delay;

        public FakeMessageHandler(int delay)
        {
            _delay = delay;
        }
        
        public void Handle(MessageQueueItem item)
        {
            //Работа по обработке сообщения, у примеру отправка в другой сервис
            Thread.Sleep(_delay);
        }
    }
}