using Common;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI_Mobile.Models;

namespace UI_Mobile
{
    public class LocalDatabase
    {
        readonly SQLiteConnection _database;

        public LocalDatabase(string dbPath)
        {
            try
            {
                _database = new SQLiteConnection(dbPath);
                _database.CreateTable<MenuItemEntity>();
                _database.CreateTable<SubItemEntity>();
                _database.CreateTable<QueueItem>();
            }
            catch (SQLiteException ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }

        public List<QueueItem> FetchQueueItems()
        {
            return _database.Table<QueueItem>().ToList();
        }

        public List<QueueItem> GetRegistrationFromBody()
        {
            return _database.Query<QueueItem>("SELECT Body FROM QueueItems");
            
        }

        public Task<int> SaveQueueItem(QueueItem queueItem)
        {
            if (queueItem.Id != 0)
            {
                return Task.FromResult(_database.Update(queueItem));
            }
            else
            {
                return Task.FromResult(_database.Insert(queueItem));
            }
        }

        public Task<int> DeleteQueueItemAsync(int id)
        {
            return Task.FromResult(_database.Delete(id));
        }

        public Task<List<MenuItemEntity>> LoadMenuItemsAsync()
        {
            var menuItems = _database.GetAllWithChildren<MenuItemEntity>();
            return Task.FromResult(menuItems);
        }

        public Task SaveMenuItemsAsync(ICollection<MenuItemEntity> items)
        {
            _database.InsertAll(items);
            ICollection<SubItemEntity> objects = items.SelectMany(i => i.SubItems).ToList();
            _database.InsertAll(objects);
            return Task.CompletedTask;
        }

        public Task DeleteAllMenuItemAsync()
         {
            _database.DropTable<MenuItemEntity>();
            _database.DropTable<SubItemEntity>();
            _database.CreateTables<MenuItemEntity, SubItemEntity>();
            return Task.CompletedTask;
        }
    }
}
