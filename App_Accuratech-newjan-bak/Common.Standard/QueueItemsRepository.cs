using Common.Standard.Context;
using Common.Standard.Contracts;
using Common.Standard.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Common.Standard
{
    public class QueueItemsRepository : IQueueItemsRepository
    {
        private readonly DatabaseContextOffline databaseContextOffline;
        public QueueItemsRepository(string dbPath)
        {
            databaseContextOffline = new DatabaseContextOffline(dbPath);
        }

        public async Task<bool> AddQueueItemsAsync(QueueEntityModel queueItem)
        {
            try
            {
                var tracking = await databaseContextOffline.QueueItems.AddAsync(queueItem);

                await databaseContextOffline.SaveChangesAsync();

                var isAdded = tracking.State == EntityState.Added;

                return isAdded;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
