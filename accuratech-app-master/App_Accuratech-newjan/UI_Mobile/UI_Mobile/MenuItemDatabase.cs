using Common;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using UI_Mobile.Models;

namespace UI_Mobile
{
    public class MenuItemDatabase
    {
        readonly SQLiteAsyncConnection _database;
        readonly SQLiteConnection _db;

        public MenuItemDatabase(string dbPath)
        {
            try
            {
                _database = new SQLiteAsyncConnection(dbPath);
                _database.CreateTableAsync<MenuItemEntity>();

            }
            catch (SQLiteException ex)
            {

                Debug.WriteLine(ex.Message);
            }

        }

        public Task<List<MenuItemEntity>> GetSubItemsAsync(List<SubItemEntity> subItemEntity)
        {
            var items = _database.Table<MenuItemEntity>().ToListAsync();
            return items;
        }

        public Task<List<MenuItemEntity>> LoadMenuItemsAsync()
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
            await _database.InsertAllAsync(items);
        }

        public async Task SaveSubItemsAsync(ICollection<SubItemEntity> items)
        {
            await _database.InsertAllAsync(items);
        }


        public async Task DeleteAllMenuItemAsync()
         {
            await _database.DropTableAsync<MenuItemEntity>();
            await _database.DropTableAsync<SubItemEntity>();
            //await _database.DropTableAsync<RegistrationItemEntity>();
            //await _database.DropTableAsync<RegistrationValueItemEntity>();

            //await _database.CreateTablesAsync<MenuItemEntity, SubItemEntity, RegistrationItemEntity, RegistrationValueItemEntity>();
        }

        public async Task DeleteAllSubItemAsync()
        {
            await _database.DropTableAsync<MenuItemEntity>();
            await _database.DropTableAsync<SubItemEntity>();
            //await _database.DropTableAsync<RegistrationItemEntity>();
            //await _database.DropTableAsync<RegistrationValueItemEntity>();

            //await _database.CreateTablesAsync<MenuItemEntity, SubItemEntity, RegistrationItemEntity, RegistrationValueItemEntity>();
        }
    }
}
