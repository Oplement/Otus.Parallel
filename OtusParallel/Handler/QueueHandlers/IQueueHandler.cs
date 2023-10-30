using System.Threading;
using System.Threading.Tasks;

namespace Otus.Teaching.Concurrency.Queue.Handler.QueueHandlers
{
    public interface IQueueHandler
    {
        void Handle();
    }
}