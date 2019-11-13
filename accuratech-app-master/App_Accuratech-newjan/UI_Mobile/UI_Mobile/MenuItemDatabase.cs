using Common;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UI_Mobile.Models;

namespace UI_Mobile
{
    public class MenuItemDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public MenuItemDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<MenuItemEntity>().Wait();
            _database.CreateTableAsync<SubItemEntity>().Wait();
            _database.CreateTableAsync<RegistrationItemEntity>().Wait();
            _database.CreateTableAsync<RegistrationValueItemEntity>().Wait();
        }

        public Task<List<MenuItemEntity>> GetMenuItemsAsync()
        {
            var items = _database.Table<MenuItemEntity>().ToListAsync();
            return items;
        }

        public Task<MenuItemEntity> GetMenuItemAsync(int id)
        {
            return _database.Table<MenuItemEntity>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public List<MenuItemEntity> FetchMenuItems()
        {
            return new List<MenuItemEntity>();
        }

        public Task<int> SaveMenuItemAsync(MenuItemEntity menuItem)
        {
            if (menuItem.Id != 0)
            {
                return _database.UpdateAsync(menuItem);
            }
            else
            {
                return _database.InsertAsync(menuItem);
            }
        }

        public async Task SaveMenuItemsAsync(ICollection<MenuItemEntity> items)
        {
            var count = 0;
            foreach (var item in items)
            {
                 count += await _database.InsertAsync(item);
            }
        }

        public Task<int> DeleteAllMenuItemAsync()
        {
            return _database.DeleteAllAsync<MenuItemEntity>();
        }
    }
}
