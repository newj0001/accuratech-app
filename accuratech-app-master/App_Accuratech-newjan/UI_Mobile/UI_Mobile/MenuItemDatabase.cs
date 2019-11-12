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
        }

        public Task<List<MenuItemEntity>> GetMenuItemsAsync()
        {
            return _database.Table<MenuItemEntity>().ToListAsync();
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

        public Task<int> DeleteMenuItemAsync(int id)
        {
            return _database.DeleteAsync(id);
        }
    }
}
