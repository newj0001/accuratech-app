using Common.Standard.Entities;
using System.Threading.Tasks;

namespace Common.Standard.Contracts
{
    public interface IQueueItemsRepository
    {
        Task<bool> AddQueueItemsAsync(QueueEntityModel queueItem);
    }
}
