using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using UI_Mobile.Models;

namespace UI_Mobile
{
    public class QueueDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public QueueDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<QueueItem>().Wait();
        }

        public Task<List<QueueItem>> GetQueueItemsAsync()
        {
            return _database.Table<QueueItem>().ToListAsync();
        }

        public Task<QueueItem> GetQueueItemAsync(int id)
        {
            return _database.Table<QueueItem>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public List<QueueItem> FetchQueueItems()
        {
            return new List<QueueItem>();
        }

        public Task<int> SaveQueueItemAsync(QueueItem queueItem)
        {
            if (queueItem.Id != 0)
            {
                return _database.UpdateAsync(queueItem);
            }
            else
            {
                return _database.InsertAsync(queueItem);
            }
        }

        public Task<int> DeleteQueueItemAsync(int id)
        {
            return _database.DeleteAsync(id);
        }
    }
}
